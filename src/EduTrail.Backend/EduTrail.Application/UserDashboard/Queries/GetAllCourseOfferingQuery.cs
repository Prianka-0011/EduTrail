using MediatR;

namespace EduTrail.Application.UserDashboard
{
    public class GetAllCourseOfferingQuery : IRequest<List<UserCourseOfferingDetail>>
    {
        public class Handler : IRequestHandler<GetAllCourseOfferingQuery,>
        {
            public Handler()
            {
                
            }
            public async Task<> handle()
            {
                
            }
        }
    }
}