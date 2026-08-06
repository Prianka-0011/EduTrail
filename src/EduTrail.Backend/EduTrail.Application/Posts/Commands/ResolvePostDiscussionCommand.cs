using AutoMapper;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class ResolvePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
        public Guid DiscussionId { get; set; }

        public bool IsResolved { get; set; }

        public class Handler
            : IRequestHandler<
                ResolvePostDiscussionCommand,
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
                ResolvePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var discussion =
                    await _repository.GetDiscussionByIdAsync(
                        request.DiscussionId);

                if (discussion == null)
                {
                    throw new KeyNotFoundException(
                        "Discussion not found.");
                }

                discussion.IsResolved =
                    request.IsResolved;

                discussion.UpdatedDate =
                    DateTimeOffset.UtcNow;

                var result =
                    await _repository.UpdateDiscussionAsync(
                        discussion);

                return _mapper.Map<PostDiscussionDto>(
                    result);
            }
        }
    }
}