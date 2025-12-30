using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Courses
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<DeleteCourseCommand,  bool>
        {
            public readonly ICourseRepository _courseRepository;
            public Handler(ICourseRepository courseRepository)
            {
                _courseRepository = courseRepository;
            }
            public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            {
                return await _courseRepository.DeleteAsync(request.Id);
            }
        }
    }
}