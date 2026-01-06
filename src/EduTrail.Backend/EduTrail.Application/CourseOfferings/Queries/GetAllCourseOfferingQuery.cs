using MediatR;

namespace EduTrail.Application.CourseOfferings
{
    public class GetAllCourseOfferingQuery : IRequest<CourseOfferingDto>
    {
        public class Handler : IRequestHandler<GetAllCourseOfferingQuery, CourseOfferingDto>
        {
            public Handler()
            {
                
            }

            public async Task<CourseOfferingDto> Handle(GetAllCourseOfferingQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
        
    }
}