using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class UpdateEnrolementCommand : IRequest<EnrolementDto>
    {
        public EnrolementDetailsDto enrolementDto {get; set;} 
        public class Handler : IRequestHandler<UpdateEnrolementCommand, EnrolementDto>
        {
            private readonly IEnrolementRepository _enrolementRepository;
            private readonly IMapper _mapper;

            public Handler(IEnrolementRepository enrolementRepository, IMapper mapper)
            {
                _enrolementRepository = enrolementRepository;
                _mapper = mapper;
            }

            public async Task<EnrolementDto> Handle (UpdateEnrolementCommand request, CancellationToken cancellationToken)
            {
                var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
                var res = await _enrolementRepository.UpdateAsync(enrolement);
                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(res);
                return new EnrolementDto { DetailsDto = enrolementDto };
            }
            
        }
    }
}