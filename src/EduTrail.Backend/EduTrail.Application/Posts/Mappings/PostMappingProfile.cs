using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            // Post DTO -> Entity
            CreateMap<PostDetailDto, Post>()
                .ForMember(dest => dest.Poll,
                    opt => opt.MapFrom(src => src.Poll))

                // FolderIds handled manually in Command
                .ForMember(dest => dest.Folders,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PostType,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Enrollments,
                    opt => opt.Ignore());


            // Post Entity -> DTO
            CreateMap<Post, PostDetailDto>()
                .ForMember(dest => dest.Poll,
                    opt => opt.MapFrom(src => src.Poll))

                // Map many-to-many folders
                .ForMember(dest => dest.FolderIds,
                    opt => opt.MapFrom(src =>
                        src.Folders.Select(x => x.Id).ToList()));



            // Poll DTO -> Entity
            CreateMap<PollDto, Poll>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PostId,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Post,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Options,
                    opt => opt.MapFrom(src => src.Options));



            // Poll Entity -> DTO
            CreateMap<Poll, PollDto>();



            // Poll Option DTO -> Entity
            CreateMap<PollOptionDto, PollOption>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PollId,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Poll,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Votes,
                    opt => opt.Ignore());



            // Poll Option Entity -> DTO
            CreateMap<PollOption, PollOptionDto>();



            // Poll Vote DTO -> Entity
            CreateMap<PollVoteDto, PollVote>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PollOption,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Enrollment,
                    opt => opt.Ignore());



            // Poll Vote Entity -> DTO
            CreateMap<PollVote, PollVoteDto>();
        }
    }
}