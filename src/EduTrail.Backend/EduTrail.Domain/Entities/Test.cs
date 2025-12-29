using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Test : IAuditable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}