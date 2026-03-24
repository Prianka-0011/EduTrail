using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetCurrentLoginUserQuery : IRequest<CurrentLoginUserDto>
    {
        public class Handler : IRequestHandler<GetCurrentLoginUserQuery, CurrentLoginUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly ICommonService _service;
            public Handler(IUserRepository userRepository, ICommonService service)
            {
                _userRepository = userRepository;
                _service = service;
            }
            public async Task<CurrentLoginUserDto> Handle(GetCurrentLoginUserQuery request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = _service._CurrentUserService.GetUserId();
                var user = await _userRepository.GetByIdAsync(currentLoginUserId);
                var userDetailDto = _service._Mapper.Map<CurrentLoginUserDto>(user);
                return userDetailDto;   
            }
        }
    }
}