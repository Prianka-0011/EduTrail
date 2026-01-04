using System.Reflection.Emit;
using System.Reflection.Metadata;
using AutoMapper;
using MediatR;

namespace EduTrail.Application.Courses
{
    public class GetCourseByIdQuery : IRequest<CourseDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetCourseByIdQuery, CourseDto>
        {
            private readonly ICourseRepository _courseRepository;
            private readonly IMapper _mapper;
            public Handler(ICourseRepository courseRepository, IMapper mapper)
            {
                _courseRepository = courseRepository;
                _mapper = mapper;
            }
            public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
            {
                var course = await _courseRepository.GetByIdAsync(request.Id);
                if(course==null)
                {
                    return new CourseDto
                    {
                        Id = Guid.Empty,
                    };
                }
                var courseDto = _mapper.Map<CourseDto>(course);
                return courseDto;
            }
        }
    }
}