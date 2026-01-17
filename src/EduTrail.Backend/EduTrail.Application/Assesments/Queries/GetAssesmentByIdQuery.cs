using AutoMapper;
using EduTrail.Application.CourseOfferings;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAssesmentByIdQuery : IRequest<AssesmentDto>
    {
        public class Handler : IRequestHandler<GetAssesmentByIdQuery, AssesmentDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
            }

            public Task<AssesmentDto> Handle(GetAssesmentByIdQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}