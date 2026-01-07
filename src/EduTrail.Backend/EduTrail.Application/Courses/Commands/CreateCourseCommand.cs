using MediatR;
using AutoMapper;
using EduTrail.Application.Courses;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Courses
{
    public class CreateCourseCommand : IRequest<CourseDto>
    {
        public CourseDto courseDto { get; set; }
        public class Handler : IRequestHandler<CreateCourseCommand, CourseDto>
        {
            private readonly ICourseRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ICourseRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {
                var course = _mapper.Map<Course>(request.courseDto);
                var result =  await _repository.CreateAsync(course);
                var courseDto = _mapper.Map<CourseDto>(result);
                return courseDto;
            }
        }
    }
}