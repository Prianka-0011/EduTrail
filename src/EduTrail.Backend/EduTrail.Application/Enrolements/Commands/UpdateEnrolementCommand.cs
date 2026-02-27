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

            // public async Task<EnrolementDto> Handle(UpdateEnrolementCommand request, CancellationToken cancellationToken)
            // {
            //     var enrolement = _mapper.Map<Enrollment>(request.enrolementDto);
            //     if (request.enrolementDto.IsTa == true)
            //     {
            //         var role = await _repository.GetRoleTaAsync();
            //         var student = enrolement.Student ?? await _repository.GetStudentByIdAsync(enrolement.StudentId ?? Guid.Empty);
            //         if (student.Roles == null)
            //         {
            //             student.Roles = new List<Role> { role };
            //         }
            //         else
            //         {
            //             student.Roles.Add(role);
            //         }

            //     }
            //     else if (request.enrolementDto.IsTa == false)
            //     {
            //         var role = await _repository.GetRoleTaAsync();
            //         var student = enrolement.Student ?? await _repository.GetStudentByIdAsync(enrolement.StudentId ?? Guid.Empty);
            //         if (student.Roles != null)
            //         {
            //             student.Roles.Remove(role);
            //         }
            //     }
            //     var res = await _repository.UpdateAsync(enrolement);
            //     var enrolementDto = _mapper.Map<EnrolementDetailsDto>(res);
            //     return new EnrolementDto { DetailsDto = enrolementDto };
            // }

            public async Task<EnrolementDto> Handle(UpdateEnrolementCommand request, CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetByIdAsync(request.enrolementDto.Id);

                if (enrolement == null)
                    throw new Exception("Enrollment not found");

                // Update basic properties
                enrolement.CourseOfferingId = request.enrolementDto.CourseOfferingId;
                enrolement.StudentId = request.enrolementDto.StudentId;
                enrolement.EnrolledDate = request.enrolementDto.EnrolledDate;
                enrolement.TotalWorkHoursPerWeek = request.enrolementDto.TotalWorkHoursPerWeek;
                enrolement.IsActive = request.enrolementDto.IsActive ?? true;


                // Update TA roles
                var student = enrolement.Student ?? await _repository.GetStudentByIdAsync(enrolement.StudentId ?? Guid.Empty);
                var taRole = await _repository.GetRoleTaAsync();

                if (request.enrolementDto.IsTa == true)
                {
                    if (student.Roles == null)
                        student.Roles = new List<Role> { taRole };
                    else if (!student.Roles.Any(r => r.Id == taRole.Id))
                        student.Roles.Add(taRole);
                }
                else
                {
                    if (student.Roles != null)
                        student.Roles.Remove(taRole);
                }

                // Update months, weeks, days, slots
                enrolement.TALabMonths.Clear();
                if (request.enrolementDto.Months != null)
                {
                    foreach (var monthDto in request.enrolementDto.Months)
                    {
                        var month = new TALabMonth
                        {
                            Id = monthDto.Id,
                            Month = monthDto.Month,
                            Year = monthDto.Year,
                            EnrollmentId = enrolement.Id,
                            Weeks = new List<TALabWeek>()
                        };

                        if (monthDto.Weeks != null)
                        {
                            foreach (var weekDto in monthDto.Weeks)
                            {
                                var week = new TALabWeek
                                {
                                    Id = weekDto.Id,
                                    WeekNumber = weekDto.WeekNumber,
                                    TALabMonthId = month.Id,
                                    Days = new List<TALabDay>()
                                };

                                if (weekDto.Days != null)
                                {
                                    foreach (var dayDto in weekDto.Days)
                                    {
                                        var day = new TALabDay
                                        {
                                            Id = dayDto.Id,
                                            LabDate = dayDto.LabDate,
                                            TALabWeekId = week.Id,
                                            IsActive = dayDto.IsActive ?? false,
                                            Slots = new List<TALabSlot>()
                                        };

                                        if (dayDto.Slots != null)
                                        {
                                            foreach (var slotDto in dayDto.Slots)
                                            {
                                                day.Slots.Add(new TALabSlot
                                                {
                                                    Id = slotDto.Id,
                                                    StartTime = slotDto.StartTime,
                                                    EndTime = slotDto.EndTime,
                                                    TALabDayId = day.Id,
                                                    Mode = slotDto.Mode.HasValue ? (LabMode)slotDto.Mode.Value : LabMode.InPerson,
                                                    RemoteLink = slotDto.RemoteLink
                                                });
                                            }
                                        }

                                        week.Days.Add(day);
                                    }
                                }

                                month.Weeks.Add(week);
                            }
                        }

                        enrolement.TALabMonths.Add(month);
                    }
                }

                var updated = await _repository.UpdateAsync(enrolement);
                var enrolementDetails = _mapper.Map<EnrolementDetailsDto>(updated);

                return new EnrolementDto { DetailsDto = enrolementDetails };
            }


        }
    }
}