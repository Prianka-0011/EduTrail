using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionVariationRule : IAuditable
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string OptionsJson { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}