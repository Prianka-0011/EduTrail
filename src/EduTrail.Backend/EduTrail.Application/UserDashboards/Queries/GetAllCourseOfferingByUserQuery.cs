using AutoMapper;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetAllCourseOfferingByUserQuery : IRequest<UserCourseOfferingDto>
    {
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
                var currentLoginUserId = Guid.Parse("C77A7ABF-BF2C-4DFF-51BD-08DE83B7A5E7");
                var res = await _repository.GetAllByUserIdAsync(currentLoginUserId);
                var dtos = _mapper.Map<List<UserCourseOfferingDetail>>(res);
                return new UserCourseOfferingDto { DetailsDtoList = dtos };
            }
        }
    }
}