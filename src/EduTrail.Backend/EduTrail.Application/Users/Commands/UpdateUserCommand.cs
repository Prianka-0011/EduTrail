using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Users
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public UserDetailDto UserDetailDto { get; set; }
        public class Handler : IRequestHandler<UpdateUserCommand, UserDto>
        {
            private readonly IUserRepository _Repository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository repository, IMapper mapper)
            {
                _Repository = repository;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request.UserDetailDto);
                var updatedUser = await _Repository.UpdateAsync(user);
                var userDetailDto = _mapper.Map<UserDetailDto>(updatedUser);
                return new UserDto { DetailDto = userDetailDto };
            }
        }
    }
}