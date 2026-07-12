using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class DeletePostCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<DeletePostCommand,  bool>
        {
            public readonly IPostRepository _postRepository;
            public Handler(IPostRepository postRepository)
            {
                _postRepository = postRepository;
            }
            public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                return await _postRepository.DeleteAsync(request.Id);
            }
        }
    }
}