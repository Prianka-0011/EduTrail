using System.ComponentModel;
using AutoMapper;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
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
                var student = await _repository.GetStudentByIdAsync(request.enrolementDto.StudentId);
                if (student == null)
                {
                    throw new InvalidOperationException("Student not found.");
                }

                if (request.enrolementDto.IsTa == true)
                {
                    student.Roles = student.Roles ?? new List<Role>();
                    if (!student.Roles.Any(r => r.Id == CustomCategory.RoleType.TA))
                    {
                        await EnsureTaRoleAsync(student);
                    }

                }
                var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
                enrolement.Student = student;
                var res = await _repository.CreateAsync(enrolement);
                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(res);
                return new EnrolementDto { DetailsDto = enrolementDto };
            }
            private async Task EnsureTaRoleAsync(User student)
            {
                student.Roles ??= new List<Role>();

                if (student.Roles.Any(r => r.Id == CustomCategory.RoleType.TA))
                    return;

                var taRole = await _repository.GetRoleTaAsync();
                student.Roles.Add(taRole);
            }
        }
    }
}