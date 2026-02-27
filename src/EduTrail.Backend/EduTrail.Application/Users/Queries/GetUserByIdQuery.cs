using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Users
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetUserByIdQuery, UserDto>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetByIdAsync(request.Id);

                var userDetailDto = _mapper.Map<UserDetailDto>(user) ?? new UserDetailDto();

                if (user?.Roles != null)
                {
                    userDetailDto.SelectedRoleList = user.Roles
                        .Select(r => new DropdownItemDto { Id = r.Id, Name = r.Name })
                        .ToList();
                }

                var roles = await _repository.GetAllRolesAsync();

                return new UserDto
                {
                    DetailDto = userDetailDto,
                    DropdownRoleList = roles.Select(r => new DropdownItemDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList()
                };
            }
        }
    }
}