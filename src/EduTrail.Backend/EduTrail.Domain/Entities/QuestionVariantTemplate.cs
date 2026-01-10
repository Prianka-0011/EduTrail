using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionVariantTemplate : IAuditable
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public string Language { get; set; }
        public string Template { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }

}