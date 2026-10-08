using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            // DTO -> Entity
            CreateMap<PostDetailDto, Post>()
                .ForMember(dest => dest.Poll,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Folders,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PostType,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Enrollments,
                    opt => opt.Ignore())
                     .ForMember(dest => dest.CreatedDate,
                opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate,
                opt => opt.Ignore());


            // Entity -> DTO
            CreateMap<Post, PostDetailDto>()
                .ForMember(dest => dest.Poll,
                    opt => opt.MapFrom(src => src.Poll))

                .ForMember(dest => dest.FolderIds,
                    opt => opt.MapFrom(src =>
                        src.Folders.Select(x => x.Id).ToList()))

                .ForMember(dest => dest.PostTypeName,
                    opt => opt.MapFrom(src => src.PostType.Name))

                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(
                    d => d.IsLiked,
                    o => o.MapFrom(s => s.UserActions.Any(x => x.IsLiked)))
                .ForMember(
                    d => d.LikeCount,
                    o => o.MapFrom(s => s.UserActions.Count(x => x.IsLiked)));
            // Poll DTO -> Entity
            CreateMap<PollDto, Poll>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())

                .ForMember(dest => dest.PostId,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Post,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Options,
                    opt => opt.Ignore());


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