using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class GetTAAndLabHoursByCourseOfferingQuery : IRequest<UserEnrollementDto>
    {
        public Guid CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetTAAndLabHoursByCourseOfferingQuery, UserEnrollementDto>
        {
            private readonly IMapper _mapper;
            private readonly IUserCourseOfferingRepository _repository;
            public Handler(IUserCourseOfferingRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<UserEnrollementDto> Handle(GetTAAndLabHoursByCourseOfferingQuery request, CancellationToken cancellationToken)
            {
                var enrolement = await _repository.GetTAByCourseOfferingAsync(request.CourseOfferingId);

                var dto = _mapper.Map<List<UserEnrollementDetailsDto>>(enrolement);

                return new UserEnrollementDto
                {
                    DetailsListDto = dto
                };
            }
        }
    }
}