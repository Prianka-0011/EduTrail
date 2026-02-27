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
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            
            public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var existingUser = await _repository.GetByIdAsync(request.UserDetailDto.Id);

                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }

                _mapper.Map(request.UserDetailDto, existingUser);

                if (request.UserDetailDto.SelectedRoleList?.Any() == true)
                {
                    var roles = await _repository.GetRolesByIdsAsync(request.UserDetailDto.SelectedRoleList);
                    existingUser.Roles = roles.ToList();
                }
                else
                {
                    existingUser.Roles.Clear();
                }

                var updatedUser = await _repository.UpdateAsync(existingUser);
                var userDetailDto = _mapper.Map<UserDetailDto>(updatedUser);

                return new UserDto { DetailDto = userDetailDto };
            }
        }
    }
}