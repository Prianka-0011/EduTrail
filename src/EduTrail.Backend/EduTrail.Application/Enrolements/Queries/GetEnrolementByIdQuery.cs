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
                var enrolementDto = _mapper.Map<EnrolementDetailsDto>(enrolement);
                var students = users.Select(u => new DropdownItemDto { Id = u.Id, Name = u.FirstName + " " + u.LastName }).ToList();
                return new EnrolementDto { DetailsDto = enrolementDto, Users = students };
            }
        }
    }
}