using AutoMapper;
using MediatR;

namespace EduTrail.Application.Assesments
{
    public class GetAllAssesmentQuery : IRequest<AssesmentDetailDto>
    {
        public class Handler : IRequestHandler<GetAllAssesmentQuery, AssesmentDetailDto>
        {
            private readonly IAssesmentRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IAssesmentRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
        }
    }
}