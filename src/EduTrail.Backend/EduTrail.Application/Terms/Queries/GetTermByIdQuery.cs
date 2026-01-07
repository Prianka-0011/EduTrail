using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Terms
{
    public class GetTermByIdQuery : IRequest<TermDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetTermByIdQuery, TermDto>
        {
            private readonly ITermRepository _repository;
            private readonly IMapper _mapper;
            public Handler(ITermRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<TermDto> Handle(GetTermByIdQuery request, CancellationToken cancellationToken)
            {
                var term = await _repository.GetByIdAsync(request.Id);
                var termDto = _mapper.Map<TermDto>(term);
                return termDto;
            }
        }
    }
}