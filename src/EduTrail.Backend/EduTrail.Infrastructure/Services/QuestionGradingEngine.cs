using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using EduTrail.Application.QuestionCatalog;

namespace EduTrail.Infrastructure.Services
{
    // Grades a submission by matching each answered field to its pl-* component in the
    // question HTML and applying the grader that component maps to:
    //   pl-number-input  -> relabs (atol/rtol tolerance)
    //   pl-multiple-choice -> exact (string equality)
    //   pl-order-blocks  -> sequence (ordered list, partial credit by position)
    //   pl-graph-select  -> set (unordered edge set, partial credit)
    // Display-only components (pl-graph, pl-bst) carry no answers-name and are skipped.
    public class QuestionGradingEngine : IQuestionGrader
    {
        private static readonly Regex ComponentTag =
            new(@"<(pl-[a-z-]+)\b([^>]*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AttrPattern =
            new(@"([\w-]+)\s*=\s*""([^""]*)""", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> GraderByTag = new(StringComparer.OrdinalIgnoreCase)
        {
            ["pl-number-input"] = "relabs",
            ["pl-multiple-choice"] = "exact",
            ["pl-order-blocks"] = "sequence",
            ["pl-graph-select"] = "set",
        };

        public Dictionary<string, FieldFeedbackDto> Grade(string questionHtml, JsonElement submittedAnswers, JsonElement correctAnswers)
        {
            var results = new Dictionary<string, FieldFeedbackDto>();
            if (submittedAnswers.ValueKind != JsonValueKind.Object)
                return results;

            var components = ParseComponents(questionHtml);

            foreach (var prop in submittedAnswers.EnumerateObject())
            {
                var name = prop.Name;

                if (!components.TryGetValue(name, out var component))
                {
                    results[name] = new FieldFeedbackDto(false, 0f, "Component not found in question HTML.");
                    continue;
                }

                if (correctAnswers.ValueKind != JsonValueKind.Object ||
                    !correctAnswers.TryGetProperty(name, out var correct))
                {
                    results[name] = new FieldFeedbackDto(false, 0f, "No correct answer found for this field.");
                    continue;
                }

                var graderType = GraderByTag.GetValueOrDefault(component.Tag, "none");
                results[name] = graderType switch
                {
                    "relabs" => GradeRelAbs(prop.Value, correct, component.Attrs),
                    "exact" => GradeExact(prop.Value, correct),
                    "sequence" => GradeSequence(prop.Value, correct),
                    "set" => GradeSet(prop.Value, correct),
                    _ => new FieldFeedbackDto(true, 1f, "Display-only component."),
                };
            }

            return results;
        }

        // ----- graders -----

        private static FieldFeedbackDto GradeRelAbs(JsonElement student, JsonElement correct, Dictionary<string, string> attrs)
        {
            double atol = ParseAttr(attrs, "atol");
            double rtol = ParseAttr(attrs, "rtol");

            var studentVals = Flatten(student);
            var correctVals = Flatten(correct);

            if (studentVals.Count == 0)
                return new FieldFeedbackDto(false, 0f, "No answer provided.");
            if (studentVals.Count != correctVals.Count)
                return new FieldFeedbackDto(false, 0f, "Answer does not match the expected shape.");

            int correctCount = 0;
            for (int i = 0; i < correctVals.Count; i++)
                if (Math.Abs(studentVals[i] - correctVals[i]) <= atol + rtol * Math.Abs(correctVals[i]))
                    correctCount++;

            float score = (float)correctCount / correctVals.Count;
            bool ok = score >= 1f;
            return new FieldFeedbackDto(ok, score, ok ? "Correct!" : "Incorrect.");
        }

        private static FieldFeedbackDto GradeExact(JsonElement student, JsonElement correct)
        {
            var s = AsString(student);
            var c = AsString(correct);
            bool ok = s == c;
            return new FieldFeedbackDto(ok, ok ? 1f : 0f, ok ? "Correct!" : "Incorrect.");
        }

        private static FieldFeedbackDto GradeSequence(JsonElement student, JsonElement correct)
        {
            var correctList = ToStringList(correct);
            var studentList = ToStringList(student);

            if (correctList.Count == 0)
                return new FieldFeedbackDto(false, 0f, "No correct answer defined.");
            if (studentList.Count == 0)
                return new FieldFeedbackDto(false, 0f, "No blocks submitted.");

            if (correctList.SequenceEqual(studentList))
                return new FieldFeedbackDto(true, 1f, "Perfect! All blocks in the correct order.");

            int matches = correctList.Zip(studentList).Count(p => p.First == p.Second);
            float score = (float)matches / correctList.Count;
            return new FieldFeedbackDto(false, score, $"{matches} of {correctList.Count} blocks in the correct position.");
        }

        private static FieldFeedbackDto GradeSet(JsonElement student, JsonElement correct)
        {
            var correctSet = ToEdgeSet(correct);
            var studentSet = ToEdgeSet(student);

            if (correctSet.Count == 0)
                return new FieldFeedbackDto(false, 0f, "No correct answer defined.");
            if (studentSet.Count == 0)
                return new FieldFeedbackDto(false, 0f, "No edges selected.");

            if (correctSet.SetEquals(studentSet))
                return new FieldFeedbackDto(true, 1f, "Correct! You selected exactly the right edges.");

            int correctSelected = studentSet.Count(e => correctSet.Contains(e));
            int wrongSelected = studentSet.Count - correctSelected;
            float score = Math.Max(0f, (float)(correctSelected - wrongSelected) / correctSet.Count);
            return new FieldFeedbackDto(false, score,
                $"{correctSelected}/{correctSet.Count} correct edges selected, {wrongSelected} incorrect.");
        }

        // ----- parsing helpers -----

        private sealed record Component(string Tag, Dictionary<string, string> Attrs);

        private static Dictionary<string, Component> ParseComponents(string html)
        {
            var map = new Dictionary<string, Component>();
            foreach (Match tag in ComponentTag.Matches(html))
            {
                var attrs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (Match attr in AttrPattern.Matches(tag.Groups[2].Value))
                    attrs[attr.Groups[1].Value] = attr.Groups[2].Value;

                if (attrs.TryGetValue("answers-name", out var name))
                    map[name] = new Component(tag.Groups[1].Value.ToLowerInvariant(), attrs);
            }
            return map;
        }

        private static double ParseAttr(Dictionary<string, string> attrs, string key)
            => attrs.TryGetValue(key, out var raw) &&
               double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var val)
                ? val
                : 0;

        private static string AsString(JsonElement el)
            => el.ValueKind == JsonValueKind.String ? el.GetString() ?? "" : el.ToString();

        private static List<string> ToStringList(JsonElement el)
            => el.ValueKind == JsonValueKind.Array
                ? el.EnumerateArray().Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() ?? "" : e.ToString()).ToList()
                : new List<string>();

        // Normalises each edge (a 2-element array) to a direction-independent "a-b" key.
        private static HashSet<string> ToEdgeSet(JsonElement el)
        {
            var result = new HashSet<string>();
            if (el.ValueKind != JsonValueKind.Array) return result;

            foreach (var item in el.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Array) continue;
                var parts = item.EnumerateArray()
                    .Select(x => x.ValueKind == JsonValueKind.String ? x.GetString() ?? "" : x.ToString())
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray();
                if (parts.Length >= 2)
                    result.Add(string.Join("-", parts));
            }
            return result;
        }

        private static List<double> Flatten(JsonElement el)
        {
            var result = new List<double>();
            FlattenInto(el, result);
            return result;
        }

        private static void FlattenInto(JsonElement el, List<double> result)
        {
            switch (el.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var item in el.EnumerateArray())
                        FlattenInto(item, result);
                    break;
                case JsonValueKind.Number:
                    result.Add(el.GetDouble());
                    break;
                case JsonValueKind.String:
                    if (double.TryParse(el.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                        result.Add(parsed);
                    break;
            }
        }
    }
}
