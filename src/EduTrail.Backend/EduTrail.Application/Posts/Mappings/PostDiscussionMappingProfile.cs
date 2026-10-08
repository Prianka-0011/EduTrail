using AutoMapper;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class PostDiscussionMappingProfile : Profile
    {
        public PostDiscussionMappingProfile()
        {
            CreateMap<CreatePostDiscussionDto, PostDiscussion>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Post,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Enrollment,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.ParentDiscussion,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Replies,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.IsResolved,
                    opt => opt.MapFrom(src => false)
                )
                .ForMember(
                    dest => dest.IsDeleted,
                    opt => opt.MapFrom(src => false)
                )
                .ForMember(
                    dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => DateTimeOffset.UtcNow)
                );
                


            CreateMap<PostDiscussion, PostDiscussionDto>()
                .ForMember(
                    dest => dest.AuthorName,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.AuthorEmail,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Replies,
                    opt => opt.MapFrom(src => src.Replies)
                )
                .ForMember(
                    dest => dest.Likes,
                    opt => opt.MapFrom(src => src.Likes)
                );
        }
    }
}