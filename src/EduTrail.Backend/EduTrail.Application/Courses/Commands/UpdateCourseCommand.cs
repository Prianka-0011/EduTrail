using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Courses
{
    public class UpdateCourseCommand : IRequest<CourseDto>
    {
       public CourseDto courseDto { get; set;}
       public class Handler : IRequestHandler<UpdateCourseCommand, CourseDto>
       {
            private readonly ICourseRepository _courseRepository;
            private readonly IMapper _mapper;

            public Handler(ICourseRepository courseRepository, IMapper mapper)
            {
                _courseRepository = courseRepository;
                _mapper = mapper;
            }

            public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                var course = _mapper.Map<Course>(request.courseDto);
                await _courseRepository.UpdateAsync(course);
                return _mapper.Map<CourseDto>(course);
            }
       } 
    }
}