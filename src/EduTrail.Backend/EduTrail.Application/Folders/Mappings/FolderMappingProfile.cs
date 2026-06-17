using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Folders
{
    public class FolderMappingProfile : Profile
    {
        public FolderMappingProfile()
        {
            CreateMap<FolderDto, Folder>();
            CreateMap<Folder, FolderDto>();
        }
    }

}