using AutoMapper;
using MediatR;

namespace EduTrail.Application.CourseOfferings
{
    public class GetAllCourseOfferingQuery : IRequest<CourseOfferingDto>
    {
        public class Handler : IRequestHandler<GetAllCourseOfferingQuery, CourseOfferingDto>
        {
            private readonly ICourseOfferingRepository _courseOfferingRepository;
            private readonly IMapper _mapper;
            public Handler(ICourseOfferingRepository courseOfferingRepository, IMapper mapper)
            {
                _courseOfferingRepository = courseOfferingRepository;
                _mapper = mapper;
            }

            public async Task<CourseOfferingDto> Handle(GetAllCourseOfferingQuery request, CancellationToken cancellationToken)
            {
                var courseOfferings = await _courseOfferingRepository.GetAllAsync();
                var courseOfferingDtos = _mapper.Map<List<CourseOfferingDetailDto>>(courseOfferings);
                return new CourseOfferingDto { DetailDto = courseOfferingDtos.FirstOrDefault() };
            }
        }
        
    }
}