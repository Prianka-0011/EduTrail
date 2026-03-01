using System.ComponentModel;
using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Terms
{
    public class CreateTermCommand : IRequest<TermDto>
    {
        public TermDetailDto TermDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateTermCommand, TermDto>
        {
            private readonly ITermRepository _termRepository;
            private readonly IMapper _mapper;
            public Handler(ITermRepository termRepository, IMapper mapper)
            {
                _termRepository = termRepository;
                _mapper = mapper;
            }
            public async Task<TermDto> Handle(CreateTermCommand request, CancellationToken cancellationToken)
            {
                var term = _mapper.Map<Term>(request.TermDetailDto);
                var res = await _termRepository.CreateAsync(term);
                var termDto = _mapper.Map<TermDetailDto>(term);
                return new TermDto { DetailDto = termDto };
            }
        }
    }
}