using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class Folder : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public Guid CourseOfferingId { get; set; }

        [ForeignKey(nameof(CourseOfferingId))]
        public CourseOffering CourseOffering { get; set; } = null!;

        // Parent Folder
        public Guid? ParentFolderId { get; set; }

        [ForeignKey(nameof(ParentFolderId))]
        public Folder? ParentFolder { get; set; }

        // Child Folders
        public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}