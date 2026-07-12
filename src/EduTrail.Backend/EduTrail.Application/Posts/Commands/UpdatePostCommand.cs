using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class UpdatePostCommand : IRequest<PostDto>
    {
        public PostDetailDto postDetailDto { get; set; }
        public class Handler : IRequestHandler<UpdatePostCommand, PostDto>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMapper _mapper;

            public Handler(IPostRepository postRepository, IMapper mapper)
            {
                _postRepository = postRepository;
                _mapper = mapper;
            }

            public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var post = _mapper.Map<Post>(request.postDetailDto);
                await _postRepository.UpdateAsync(post);
                var postDetailDto = _mapper.Map<PostDetailDto>(post);
                return new PostDto
                {
                    DetailsDto = postDetailDto
                };
            }
        }
    }
}