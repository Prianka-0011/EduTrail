using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class QuestionLine : IAuditable
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }

        public string CodeLine { get; set; }

        public int CorrectOrder { get; set; }

        public bool IsMovable { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}