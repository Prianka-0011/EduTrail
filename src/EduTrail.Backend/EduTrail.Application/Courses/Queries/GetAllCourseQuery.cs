using MediatR;
using AutoMapper;
using EduTrail.Application.Courses;
using System.Security.Cryptography.X509Certificates;
namespace EduTrail.Application.Courses
{
    public class GetAllCoursesQuery : IRequest<List<CourseDto>>
    {
        public class Handler : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
        {
            private readonly ICourseRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ICourseRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = await _repository.GetAllAsync();
                var courseDtos = _mapper.Map<List<CourseDto>>(courses);
                return courseDtos;
            }
        }
    }
}