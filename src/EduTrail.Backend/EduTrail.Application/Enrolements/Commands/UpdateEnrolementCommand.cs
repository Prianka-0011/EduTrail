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

                enrolement.CourseOfferingId = request.enrolementDto.CourseOfferingId;
                enrolement.StudentId = request.enrolementDto.StudentId;
                enrolement.EnrolledDate = request.enrolementDto.EnrolledDate;
                enrolement.TotalWorkHoursPerWeek = request.enrolementDto.TotalWorkHoursPerWeek;
                enrolement.IsActive = request.enrolementDto.IsActive ?? true;

                var student = enrolement.Student!;
                var taRole = await _repository.GetRoleTaAsync();

                if (request.enrolementDto.IsTa == true)
                {
                    if (!student.Roles.Any(r => r.Id == taRole.Id))
                        student.Roles.Add(taRole);
                }
                else
                {
                    var roleToRemove = student.Roles.FirstOrDefault(r => r.Id == taRole.Id);
                    if (roleToRemove != null)
                        student.Roles.Remove(roleToRemove);
                }

                var existingMonths = enrolement.TALabMonths.ToList();

                foreach (var month in existingMonths)
                {
                    if (!request.enrolementDto.Months.Any(m => m.Id == month.Id))
                        enrolement.TALabMonths.Remove(month);
                }

                foreach (var monthDto in request.enrolementDto.Months)
                {
                    var month = enrolement.TALabMonths
                        .FirstOrDefault(m => m.Id == monthDto.Id);

                    if (month == null)
                    {
                        month = new TALabMonth
                        {
                            Month = monthDto.Month,
                            Year = monthDto.Year,
                            Weeks = new List<TALabWeek>()
                        };
                        enrolement.TALabMonths.Add(month);
                    }

                    var existingWeeks = month.Weeks.ToList();
                    foreach (var week in existingWeeks)
                    {
                        if (!monthDto.Weeks.Any(w => w.Id == week.Id))
                            month.Weeks.Remove(week);
                    }

                    foreach (var weekDto in monthDto.Weeks)
                    {
                        var week = month.Weeks.FirstOrDefault(w => w.Id == weekDto.Id);

                        if (week == null)
                        {
                            week = new TALabWeek
                            {
                                WeekNumber = weekDto.WeekNumber,
                                Days = new List<TALabDay>()
                            };
                            month.Weeks.Add(week);
                        }

                        var existingDays = week.Days.ToList();
                        foreach (var day in existingDays)
                        {
                            if (!weekDto.Days.Any(d => d.Id == day.Id))
                                week.Days.Remove(day);
                        }

                        foreach (var dayDto in weekDto.Days)
                        {
                            var day = week.Days.FirstOrDefault(d => d.Id == dayDto.Id);

                            if (day == null)
                            {
                                day = new TALabDay
                                {
                                    LabDate = dayDto.LabDate,
                                    IsActive = dayDto.IsActive ?? false,
                                    Slots = new List<TALabSlot>()
                                };
                                week.Days.Add(day);
                            }

                            var existingSlots = day.Slots.ToList();
                            foreach (var slot in existingSlots)
                            {
                                if (!dayDto.Slots.Any(s => s.Id == slot.Id))
                                    day.Slots.Remove(slot);
                            }

                            foreach (var slotDto in dayDto.Slots)
                            {
                                var slot = day.Slots.FirstOrDefault(s => s.Id == slotDto.Id);

                                if (slot == null)
                                {
                                    slot = new TALabSlot();
                                    day.Slots.Add(slot);
                                }

                                slot.StartTime = slotDto.StartTime;
                                slot.EndTime = slotDto.EndTime;
                                slot.Mode = slotDto.Mode.HasValue
                                    ? (LabMode)slotDto.Mode.Value
                                    : LabMode.InPerson;
                                slot.RemoteLink = slotDto.RemoteLink;
                            }
                        }
                    }
                }
                await _repository.UpdateAsync(enrolement);
                var result = _mapper.Map<EnrolementDetailsDto>(enrolement);
                return new EnrolementDto { DetailsDto = result };
            }
        }
    }
}