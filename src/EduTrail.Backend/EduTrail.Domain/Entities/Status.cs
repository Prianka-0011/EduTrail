using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class Status : IAuditable
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid StatusTypeId { get; set; }
        public StatusType StatusType { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}