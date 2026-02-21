using AutoMapper;
using EduTrail.Application.Terms;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class GetEnrolementByIdQuery : IRequest<EnrolementDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetEnrolementByIdQuery, EnrolementDto>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<EnrolementDto> Handle(GetEnrolementByIdQuery request, CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetByIdAsync(request.Id);
                var enrolementDto = _mapper.Map<EnrolementDto>(enrolement);
                return enrolementDto;
            }
        }
    }
}