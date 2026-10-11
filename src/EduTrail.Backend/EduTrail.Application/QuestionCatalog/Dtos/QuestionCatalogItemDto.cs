namespace EduTrail.Application.QuestionCatalog
{
    // A single entry in the PrairieLearn-style question catalog, built from a
    // question folder's info.json. Mirrors the reference project's QuestionSummary.
    public class QuestionCatalogItemDto
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }
}
