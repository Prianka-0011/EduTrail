using System.Globalization;
using AutoMapper;
using EduTrail.Application.UserDashboards;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.UserDashboards
{
    public class SubmitHelpRequestCommand : IRequest<HelpRequestDto>
    {
        public HelpRequestDetailDto HelpRequest { get; set; }
        public class Handler : IRequestHandler<SubmitHelpRequestCommand, HelpRequestDto>
        {
            private readonly IUserCourseOfferingRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUserCourseOfferingRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<HelpRequestDto> Handle(SubmitHelpRequestCommand request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = Guid.Parse("BE7CC9F8-7150-4E22-9137-08DE77468F3B");
                var labRequest = _mapper.Map<LabRequest>(request.HelpRequest);
                labRequest.StudentId = currentLoginUserId;
                labRequest.StatusId = CustomCategory.HelpRequestStatus.Pending;
                await _repository.CreateHelpRequestAsync(labRequest);
                var result = _mapper.Map<HelpRequestDetailDto>(labRequest);
                return new HelpRequestDto { DetailsDto = result };
            }
        }
    }
}