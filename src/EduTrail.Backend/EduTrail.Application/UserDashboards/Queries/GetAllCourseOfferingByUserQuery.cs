using AutoMapper;
using EduTrail.Application.Shared;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetAllCourseOfferingByUserQuery : IRequest<UserCourseOfferingDto>
    {
        public class Handler : IRequestHandler<GetAllCourseOfferingByUserQuery, UserCourseOfferingDto>
        {
            private readonly ICommonService _service;
            private readonly IUserCourseOfferingRepository _repository;
            public Handler(IUserCourseOfferingRepository repository, ICommonService service)
            {
                _repository = repository;
                _service = service;
            }
            public async Task<UserCourseOfferingDto> Handle(GetAllCourseOfferingByUserQuery request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = _service._CurrentUserService.GetUserId();
                var res = await _repository.GetAllByUserIdAsync(currentLoginUserId);
                var dtos = _service._Mapper.Map<List<UserCourseOfferingDetail>>(res);
                return new UserCourseOfferingDto { DetailsDtoList = dtos };
            }
        }
    }
}