using EduTrail.Domain.Interfaces;

namespace EduTrail.Application.Assesments
{
    public class Assesment : IAuditable
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}