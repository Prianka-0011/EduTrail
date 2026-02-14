using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.CourseOfferings
{
    public class CreateCourseOfferingCommand : IRequest<CourseOfferingDto>
    {
        public CourseOfferingDetailDto CourseOfferingDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateCourseOfferingCommand, CourseOfferingDto>
        {
            private readonly ICourseOfferingRepository _courseOfferingRepository;
            private readonly IMapper _mapper;
            public Handler(ICourseOfferingRepository courseOfferingRepository, IMapper mapper)
            {
                _courseOfferingRepository = courseOfferingRepository;
                _mapper = mapper;
            }

            public async Task<CourseOfferingDto> Handle(CreateCourseOfferingCommand request, CancellationToken cancellationToken)
            {
                var courseOffering = _mapper.Map<CourseOffering>(request.CourseOfferingDetailDto);
                var createdCourseOffering = await _courseOfferingRepository.CreateAsync(courseOffering);
                var courseOfferingDto = _mapper.Map<CourseOfferingDetailDto>(createdCourseOffering);
                return new CourseOfferingDto { DetailDto = courseOfferingDto };
            }
        }
    }
}