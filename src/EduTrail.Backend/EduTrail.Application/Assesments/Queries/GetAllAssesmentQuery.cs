using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAllAssesmentQuery : IRequest<AssesmentDetailDto>
    {
        public class Handler : IRequestHandler<GetAllAssesmentQuery, AssesmentDetailDto>
        {
            public Handler()
            {
                
            }
        }
    }
}