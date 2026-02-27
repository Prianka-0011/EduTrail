using System.ComponentModel;
using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class CreateEnrolementCommand : IRequest<EnrolementDto>
    {
        public EnrolementDetailsDto enrolementDto { get; set; }
        public class Handler : IRequestHandler<CreateEnrolementCommand, EnrolementDto>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<EnrolementDto> Handle(CreateEnrolementCommand request, CancellationToken cancellationToken)
            {
                var existing = await _repository.GetByCourseOfferingIdAndStudentIdAsync(request.enrolementDto.CourseOfferingId, request.enrolementDto.StudentId);
                if (existing != null)
                {
                    throw new InvalidOperationException(

                        "Student is already enrolled in this course offering."
                    );
                }
                var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
                if (request.enrolementDto.IsTa == true)
                {
                    var role = await _repository.GetRoleTaAsync();
                    enrolement.Student?.Roles.Add(role);
                }
                
                var res = await _repository.CreateAsync(enrolement);
                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(res);
                return new EnrolementDto { DetailsDto = enrolementDto };
            }
        }
    }
}