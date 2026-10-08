using AutoMapper;
using EduTrail.Application.Questions;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Folders
{
    public class FolderMappingProfile : Profile
    {
        public FolderMappingProfile()
        {
            CreateMap<FolderDetailsDto, Folder>();
            CreateMap<Folder, FolderDetailsDto>();
        }
    }

}