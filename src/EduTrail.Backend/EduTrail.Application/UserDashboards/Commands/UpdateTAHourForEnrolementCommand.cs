using AutoMapper;
using EduTrail.Application.UserDashboards;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class UpdateTAHourForEnrolementCommand : IRequest<UserEnrollementDto>
    {
        public UserEnrollementDetailsDto enrolementDto { get; set; }
        public class Handler : IRequestHandler<UpdateTAHourForEnrolementCommand, UserEnrollementDto>
        {
            private readonly IUserCourseOfferingRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUserCourseOfferingRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<UserEnrollementDto> Handle(UpdateTAHourForEnrolementCommand request, CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetByIdAsync(request.enrolementDto.Id);

                if (enrolement == null)
                    throw new Exception("Enrollment not found");

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
                var result = _mapper.Map<UserEnrollementDetailsDto>(enrolement);
                return new UserEnrollementDto { DetailsDto = result };
            }
        }
    }
}