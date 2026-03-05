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
                var currentLoginUserId = Guid.Parse("8D1B9DFD-511C-42ED-3F21-08DE77033B15");
                var res = await _repository.GetAllByUserIdAsync(currentLoginUserId);
                var dtos = _mapper.Map<List<UserCourseOfferingDetail>>(res);
                return new UserCourseOfferingDto { DetailsDtoList = dtos };
            }
        }
    }
}