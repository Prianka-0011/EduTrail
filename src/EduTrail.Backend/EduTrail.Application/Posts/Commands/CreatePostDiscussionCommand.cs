using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class CreatePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
        public CreatePostDiscussionDto DiscussionDto { get; set; } = new();

        public Guid CreatedById { get; set; }

        public Guid PostId =>
            DiscussionDto.PostId;


        public class Handler
            : IRequestHandler<
                CreatePostDiscussionCommand,
                PostDiscussionDto>
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

            public async Task<PostDiscussionDto> Handle(
                CreatePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var dto = request.DiscussionDto;

                var discussion =
                    _mapper.Map<PostDiscussion>(dto);

                discussion.Id = Guid.NewGuid();

                discussion.CreatedById =
                    request.CreatedById;

                discussion.CreatedDate =
                    DateTimeOffset.UtcNow;

                discussion.IsDeleted = false;

                discussion.IsResolved = false;

                var result =
                    await _repository.CreateDiscussionAsync(
                        discussion);

                return _mapper.Map<PostDiscussionDto>(
                    result);
            }
        }
    }
}