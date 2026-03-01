using AutoMapper;
using EduTrail.Application.Shared.Dtos;
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
                var termDetailDto = _mapper.Map<TermDetailDto>(term);
                var types = await _repository.GetTermTypesAsync();
                var termDto = new TermDto
                {
                    DetailDto = termDetailDto ?? new TermDetailDto { Id = request.Id, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(4) },
                    Types = types.Select(t => new DropdownItemDto { Id = t.Id, Name = t.Name }).ToList()
                };
                return termDto;
            }
        }
    }
}