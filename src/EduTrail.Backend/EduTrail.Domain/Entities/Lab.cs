using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class Lab :  IAuditable
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        [Required]
        [ForeignKey("CourseOffering")]
        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}