using MediatR;

namespace EduTrail.Application.Posts
{
    public class DeletePostDiscussionCommand
        : IRequest<bool>
    {
        public Guid Id { get; set; }


        public class Handler
            : IRequestHandler<
                DeletePostDiscussionCommand,
                bool>
        {
            private readonly IPostRepository _repository;

            public Handler(
                IPostRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(
                DeletePostDiscussionCommand request,
                CancellationToken cancellationToken)
            {
                var discussion =
                    await _repository.GetDiscussionByIdAsync(
                        request.Id);

                if (discussion == null)
                {
                    return false;
                }

                discussion.IsDeleted = true;

                discussion.UpdatedDate =
                    DateTimeOffset.UtcNow;

                await _repository.UpdateDiscussionAsync(
                    discussion);

                return true;
            }
        }
    }
}