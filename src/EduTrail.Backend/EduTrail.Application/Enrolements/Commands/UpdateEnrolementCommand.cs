using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class UpdateEnrolementCommand : IRequest<EnrolementDto>
    {
        public EnrolementDetailsDto enrolementDto { get; set; }
        public class Handler : IRequestHandler<UpdateEnrolementCommand, EnrolementDto>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IEnrolementRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<EnrolementDto> Handle(UpdateEnrolementCommand request, CancellationToken cancellationToken)
            {
                var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
                if (request.enrolementDto.IsTa == true)
                {
                    var role = await _repository.GetRoleTaAsync();
                    var student = enrolement.Student ?? await _repository.GetStudentByIdAsync(enrolement.StudentId ?? Guid.Empty);
                    if (student.Roles == null)
                    {
                        student.Roles = new List<Role> { role };
                    }
                    else
                    {
                        student.Roles.Add(role);
                    }

                }
                else if (request.enrolementDto.IsTa == false)
                {
                    var role = await _repository.GetRoleTaAsync();
                    var student = enrolement.Student ?? await _repository.GetStudentByIdAsync(enrolement.StudentId ?? Guid.Empty);
                    if (student.Roles != null)
                    {
                        student.Roles.Remove(role);
                    }
                }
                var res = await _repository.UpdateAsync(enrolement);
                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(res);
                return new EnrolementDto { DetailsDto = enrolementDto };
            }

        }
    }
}