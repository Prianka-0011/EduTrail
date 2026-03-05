using AutoMapper;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetEnrollmentByUserId : IRequest<UserEnrollementDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetEnrollmentByUserId, UserEnrollementDto>
        {
            private readonly IMapper _mapper;
            private readonly IUserCourseOfferingRepository _repository;
            public Handler(IUserCourseOfferingRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<UserEnrollementDto> Handle(GetEnrollmentByUserId request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = Guid.Parse("8D1B9DFD-511C-42ED-3F21-08DE77033B15");

                var res = await _repository.GetEnrollmentByUserIdAsync(currentLoginUserId, request.CourseOfferingId);

                var dto = _mapper.Map<UserEnrollementDetailsDto>(res);

                return new UserEnrollementDto
                {
                    detailsDto = dto
                };
            }
        }
    }
}