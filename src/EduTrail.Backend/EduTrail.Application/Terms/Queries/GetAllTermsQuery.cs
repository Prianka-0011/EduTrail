using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using MediatR;

namespace EduTrail.Application.Terms
{
    public class GetAllTermsQuery : IRequest<TermDto>
    {
        public class Handler : IRequestHandler<GetAllTermsQuery, TermDto>
        {
            private readonly ITermRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ITermRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<TermDto> Handle(GetAllTermsQuery request, CancellationToken cancellationToken)
            {
                var terms = await _repository.GetAllAsync();
                var termDtos = _mapper.Map<List<TermDetailDto>>(terms);
                return new TermDto { DetailDtoList = termDtos };
            }
        }
    }
}