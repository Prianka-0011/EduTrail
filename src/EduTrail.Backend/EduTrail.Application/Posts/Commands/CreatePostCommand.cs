using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class CreatePostCommand : IRequest<PostDto>
    {
        public PostDetailDto PostDetailDto { get; set; } = new();


        public class Handler : IRequestHandler<CreatePostCommand, PostDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;

            public Handler(
                IPostRepository repository,
                IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }


            public async Task<PostDto> Handle(
                CreatePostCommand request,
                CancellationToken cancellationToken)
            {
                var dto = request.PostDetailDto;


                // Map basic Post fields
                var post = _mapper.Map<Post>(dto);


                // Map folders (many-to-many relationship)
                if (dto.FolderIds != null && dto.FolderIds.Any())
                {
                    post.Folders = await _repository
                        .GetFoldersByIdsAsync(dto.FolderIds);
                }


                // Map Poll
                if (dto.Poll != null)
                {
                    post.Poll = new Poll
                    {
                        Id = Guid.NewGuid(),
                        Question = dto.Poll.Question,

                        Options = dto.Poll.Options
                            .Select(option => new PollOption
                            {
                                Id = Guid.NewGuid(),
                                OptionText = option.OptionText,
                                VoteCount = 0
                            })
                            .ToList()
                    };
                }


                var result = await _repository.CreateAsync(post);


                var postDto = _mapper.Map<PostDetailDto>(result);


                return new PostDto
                {
                    DetailsDto = postDto
                };
            }
        }
    }
}