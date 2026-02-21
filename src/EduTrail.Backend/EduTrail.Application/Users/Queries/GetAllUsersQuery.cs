using AutoMapper;
using MediatR;

namespace EduTrail.Application.Users
{
    public class GetAllUsersQuery : IRequest<UserDto>
    {
        public class Handler : IRequestHandler<GetAllUsersQuery, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public async Task<UserDto> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetAllAsync();
                var userDetailDtos = _mapper.Map<List<UserDetailDto>>(users);
               
                return new UserDto
                {
                    DetailDtoList = userDetailDtos
                };
            }
        }
    }
}