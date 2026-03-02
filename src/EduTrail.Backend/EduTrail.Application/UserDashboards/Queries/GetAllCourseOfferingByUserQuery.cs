using AutoMapper;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetAllCourseOfferingByUserQuery : IRequest<UserCourseOfferingDto>
    {
        public Guid UserId { get; set; }
        public class Handler : IRequestHandler<GetAllCourseOfferingByUserQuery, UserCourseOfferingDto>
        {
            private readonly IMapper _mapper;
            private readonly IUserCourseOfferingRepository _repository;
            public Handler(IUserCourseOfferingRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<UserCourseOfferingDto> Handle(GetAllCourseOfferingByUserQuery request, CancellationToken cancellationToken)
            {
                var res = await _repository.GetAllByUserIdAsync(request.UserId);
                var dtos = _mapper.Map<List<UserCourseOfferingDetail>>(res);
                return new UserCourseOfferingDto { DetailsDtoList = dtos };
            }
        }
    }
}