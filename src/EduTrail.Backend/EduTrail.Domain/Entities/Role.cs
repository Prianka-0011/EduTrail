using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class Role : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(70)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        // Audit fields
        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

