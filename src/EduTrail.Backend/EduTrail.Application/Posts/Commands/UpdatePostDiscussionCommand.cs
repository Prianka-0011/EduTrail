using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class UpdatePostDiscussionCommand
        : IRequest<PostDiscussionDto>
    {
       public CreatePostDiscussionDto DiscussionDto { get; set; } = new();


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
                        request.DiscussionDto.Id ?? Guid.Empty);

                if (discussion == null)
                {
                    throw new KeyNotFoundException(
                        "Discussion not found.");
                }

                discussion.Content =
                    request.DiscussionDto.Content;

                discussion.EditorType =
                    request.DiscussionDto.EditorType;

                discussion.IsResolved =
                    request.DiscussionDto.IsResolved ?? false;

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