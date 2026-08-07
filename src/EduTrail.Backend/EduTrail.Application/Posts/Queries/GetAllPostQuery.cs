using MediatR;
using AutoMapper;

namespace EduTrail.Application.Posts
{
    public class GetAllPostQuery : IRequest<PostDto>
    {
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<GetAllPostQuery, PostDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IPostRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<PostDto> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
            {
                var posts = await _repository.GetAllAsync(request.CourseOfferingId);
                var postDtos = _mapper.Map<List<PostDetailDto>>(posts);

                return new PostDto
                {
                    DetailsListDto = postDtos
                };
            }
        }
    }
}