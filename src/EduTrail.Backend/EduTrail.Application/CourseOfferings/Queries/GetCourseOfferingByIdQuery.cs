using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using MediatR;

namespace EduTrail.Application.CourseOfferings
{
    public class GetCourseOfferingByIdQuery : IRequest<CourseOfferingDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetCourseOfferingByIdQuery, CourseOfferingDto>
        {
            private readonly ICourseOfferingRepository _courseOfferingRepository;
            private readonly IMapper _mapper;
            public Handler(ICourseOfferingRepository courseOfferingRepository, IMapper mapper)
            {
                _courseOfferingRepository = courseOfferingRepository;
                _mapper = mapper;
            }

            public async Task<CourseOfferingDto> Handle(GetCourseOfferingByIdQuery request, CancellationToken cancellationToken)
            {
                var courseOffering = await _courseOfferingRepository.GetCourseOfferingById(request.Id);
                var courseOfferingDto = _mapper.Map<CourseOfferingDetailDto>(courseOffering);
                var courses = await _courseOfferingRepository.GetAllCourses();
                var courseDropdownItems = courses.Select(c => new DropdownItemDto { Id = c.Id, Name = c.CourseName+"("+c.CourseCode+")" }).ToList();
                var terms = await _courseOfferingRepository.GetAllTerms();
                var termDropdownItems = terms.Select(t => new DropdownItemDto { Id = t.Id, Name = t.Name+"("+t.Year+")" }).ToList();
                var instructors = await _courseOfferingRepository.GetAllInstructors();
                var instructorDropdownItems = instructors.Select(i => new DropdownItemDto { Id = i.Id, Name = i.FirstName + " " + i.LastName }).ToList();
               
                return new CourseOfferingDto { DetailDto = courseOfferingDto, Courses = courseDropdownItems, Terms = termDropdownItems, Instructors = instructorDropdownItems };
            }
        }
    }
}