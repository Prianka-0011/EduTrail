using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
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

            public async Task<EnrolementDto> Handle(
                GetEnrolementByIdQuery request,
                CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetByIdAsync(request.Id);
                var users = await _repository.GetAllUsersAsync();

                var students = users.Select(u => new DropdownItemDto
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                }).ToList();

                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(enrolement)
                    ?? new EnrolementDetailsDto
                    {
                        Id = request.Id,
                        EnrolledDate = DateTime.UtcNow
                    };

                if (enrolement == null || enrolement.CourseOffering == null || enrolement.CourseOffering.Term == null)
                {
                    return new EnrolementDto
                    {
                        DetailsDto = enrolementDto,
                        Users = students,
                        DropdownMonths = new List<DropdownItemDtoInt>()
                    };
                }

                var taRole = await _repository.GetRoleTaAsync();
                enrolementDto.IsTa =
                    enrolement.Student?.Roles?.Any(r => r.Id == taRole.Id) == true;

                return new EnrolementDto
                {
                    DetailsDto = enrolementDto,
                    Users = students,
                    DropdownMonths = GetMonthsByTerm(enrolement.CourseOffering.Term.TermTypeId)
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
