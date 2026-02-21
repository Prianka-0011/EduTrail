using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Users
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public UserDetailDto UserDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request.UserDetailDto);
                if (request.UserDetailDto.SelectedRoleList?.Any() == true)
                {
                    var roles = await _repository.GetRolesByIdsAsync(request.UserDetailDto.SelectedRoleList);
                    user.Roles = roles.ToList();
                }
                var createdUser = await _repository.CreateAsync(user);
                var userDetailDto = _mapper.Map<UserDetailDto>(createdUser);
                return new UserDto { DetailDto = userDetailDto };
            }
        }
    }
}