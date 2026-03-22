using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetEnrollmentByUserId : IRequest<UserEnrollementDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetEnrollmentByUserId, UserEnrollementDto>
        {
            private readonly ICommonService _service;
            private readonly IUserCourseOfferingRepository _repository;
            public Handler(IUserCourseOfferingRepository repository, ICommonService service)
            {
                _repository = repository;
                _service = service;
            }
            public async Task<UserEnrollementDto> Handle(GetEnrollmentByUserId request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = _service._CurrentUserService.GetUserId();
                var enrolement = await _repository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.CourseOfferingId);

                var dto = _service._Mapper.Map<UserEnrollementDetailsDto>(enrolement) ?? new UserEnrollementDetailsDto();

                dto.IsTa = enrolement.User.Roles.Any(c => c.Id == CustomCategory.RoleType.TA) ? true : false;
                return new UserEnrollementDto
                {
                    DetailsDto = dto,
                    DropdownMonths = GetMonthsByTerm(enrolement.CourseOffering.Term.TermTypeId),
                    Year = enrolement.CourseOffering.Term.Year,
                };
            }
            private static List<DropdownItemDtoInt> GetMonthsByTerm(Guid termTypeId)
            {
                if (termTypeId == CustomCategory.TermType.Fall)
                {
                    return new List<DropdownItemDtoInt>
                    {
                        new() { Id = CustomCategory.Months.September, Name = "September" },
                        new() { Id = CustomCategory.Months.October,   Name = "October" },
                        new() { Id = CustomCategory.Months.November,  Name = "November" }
                    };
                }

                if (termTypeId == CustomCategory.TermType.Spring)
                {
                    return new List<DropdownItemDtoInt>
                    {
                        new() { Id = CustomCategory.Months.March, Name = "March" },
                        new() { Id = CustomCategory.Months.April, Name = "April" },
                        new() { Id = CustomCategory.Months.May,   Name = "May" }
                    };
                }

                if (termTypeId == CustomCategory.TermType.Winter)
                {
                    return new List<DropdownItemDtoInt>
                    {
                        new() { Id = CustomCategory.Months.December, Name = "December" },
                        new() { Id = CustomCategory.Months.January,  Name = "January" },
                        new() { Id = CustomCategory.Months.February, Name = "February" }
                    };
                }

                return new List<DropdownItemDtoInt>();
            }
        }
    }
}