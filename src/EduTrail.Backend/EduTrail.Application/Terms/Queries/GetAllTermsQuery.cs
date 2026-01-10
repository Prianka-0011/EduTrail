using AutoMapper;
using MediatR;

namespace EduTrail.Application.Terms
{
    public class GetAllTermsQuery : IRequest<List<TermDto>>
    {
        public class Handler : IRequestHandler<GetAllTermsQuery, List<TermDto>>
        {
            private readonly ITermRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ITermRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<TermDto>> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
            {
                var terms = await _repository.GetAllAsync();
                var termDtos = _mapper.Map<List<TermDto>>(terms);
                return termDtos;
            }
        }
    }
}