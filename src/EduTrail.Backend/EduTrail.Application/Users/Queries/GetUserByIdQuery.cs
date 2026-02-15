using AutoMapper;
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
                var userDetailDto = _mapper.Map<UserDetailDto>(user);

                return new UserDto
                {
                    DetailDto = userDetailDto
                };
            }
        }
    }
}