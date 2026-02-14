using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.CourseOfferings
{
    public class UpdateCourseOfferingCommand : IRequest<CourseOfferingDto>
    {
        public CourseOfferingDetailDto CourseOfferingDetailDto { get; set; }
        public class Handler : IRequestHandler<UpdateCourseOfferingCommand, CourseOfferingDto>
        {
            private readonly ICourseOfferingRepository _courseOfferingRepository;
            private readonly IMapper _mapper;
            public Handler(ICourseOfferingRepository courseOfferingRepository, IMapper mapper)
            {
                _courseOfferingRepository = courseOfferingRepository;
                _mapper = mapper;
            }

            public async Task<CourseOfferingDto> Handle(UpdateCourseOfferingCommand request, CancellationToken cancellationToken)
            {
                var courseOffering = _mapper.Map<CourseOffering>(request.CourseOfferingDetailDto);
                var updatedCourseOffering = await _courseOfferingRepository.UpdateAsync(courseOffering);
                var courseOfferingDto = _mapper.Map<CourseOfferingDetailDto>(updatedCourseOffering);
                return new CourseOfferingDto { DetailDto = courseOfferingDto };
            }
        }
    }
}