using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Terms;
using EduTrail.Domain.Entities;
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

            public async Task<EnrolementDto> Handle(GetEnrolementByIdQuery request, CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetByIdAsync(request.Id);
                var users = await _repository.GetAllUsersAsync();
                var students = users.Select(u => new DropdownItemDto
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                }).ToList();

                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(enrolement) ?? new EnrolementDetailsDto
                {
                    Id = request.Id,
                    EnrolledDate = DateTime.UtcNow
                };

                if (enrolement == null)
                    return new EnrolementDto { DetailsDto = enrolementDto, Users = students };

                var taRole = await _repository.GetRoleTaAsync();
                if (enrolement.Student?.Roles?.Any(r => r.Id == taRole.Id) == true)
                {
                    enrolementDto.IsTa = true;
                }
                return new EnrolementDto { DetailsDto = enrolementDto, Users = students };
            }
        }
    }
}