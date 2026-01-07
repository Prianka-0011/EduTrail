using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Terms
{
    public class UpdateTermCommand : IRequest<TermDto>
    {
        public TermDto termDto {get; set;} 
        public class Handler : IRequestHandler<UpdateTermCommand, TermDto>
        {
            private readonly ITermRepository _termRepository;
            private readonly IMapper _mapper;

            public Handler(ITermRepository termRepository, IMapper mapper)
            {
                _termRepository = termRepository;
                _mapper = mapper;
            }

            public async Task<TermDto> Handle (UpdateTermCommand request, CancellationToken cancellationToken)
            {
                var term = _mapper.Map<Term>(request.termDto);
                var res = await _termRepository.UpdateAsync(term);
                var termDto = _mapper.Map<TermDto>(res);
                return termDto;
            }
            
        }
    }
}