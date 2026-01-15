using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAllAssesmentQuery : IRequest<AssesmentDetailDto>
    {
        public class Handler : IRequestHandler<GetAllAssesmentQuery, AssesmentDetailDto>
        {
            private readonly 
            public Handler()
            {
                
            }
        }
    }
}