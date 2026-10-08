using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Folders
{
    public class FolderDto
    {
        public bool? IsSuccess { get; set; }
        public string? Message {get; set;}
        public FolderDetailsDto DetailsDto { get; set; } = new FolderDetailsDto();

        public List<FolderDetailsDto> DetailsListDto { get; set; } = new List<FolderDetailsDto>();

    }
    public class FolderDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public Guid CourseOfferingId { get; set; }
        public Guid? ParentFolderId { get; set; }
        // Child Folders
        public ICollection<FolderDetailsDto> SubFolders { get; set; } = new List<FolderDetailsDto>();

    }
  
}

