using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class UpdatePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public string Content { get; set; } = string.Empty;

        public EditorType EditorType { get; set; }

        public bool IsResolved { get; set; }


        public class Handler
            : IRequestHandler<
                UpdatePostDiscussionCommand,
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
                UpdatePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var discussion =
                    await _repository.GetDiscussionByIdAsync(
                        request.Id);

                if (discussion == null)
                {
                    throw new KeyNotFoundException(
                        "Discussion not found.");
                }

                discussion.Content =
                    request.Content;

                discussion.EditorType =
                    request.EditorType;

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