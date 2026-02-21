using System.ComponentModel;
using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class CreateEnrolementCommand : IRequest<EnrolementDto>
    {
        public EnrolementDto enrolementDto { get; set; }
        public class Handler : IRequestHandler<CreateEnrolementCommand, EnrolementDto>
        {
            private readonly IEnrolementRepository _enrolementRepository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository enrolementRepository, IMapper mapper)
            {
                _enrolementRepository = enrolementRepository;
                _mapper = mapper;
            }
            public async Task<EnrolementDto> Handle(CreateEnrolementCommand request, CancellationToken cancellationToken)
            {
                var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
                var res = await _enrolementRepository.CreateAsync(enrolement);
                var enrolementDto = _mapper.Map<EnrolementDto>(enrolement);
                return enrolementDto;
            }
        }
    }
}