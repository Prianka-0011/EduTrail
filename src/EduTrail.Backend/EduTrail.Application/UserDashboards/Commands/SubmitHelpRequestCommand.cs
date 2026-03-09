using AutoMapper;
using EduTrail.Application.UserDashboards;
using EduTrail.Domain.Entities;
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
                var labRequest = _mapper.Map<LabRequest>(request.HelpRequest);
                await _repository.CreateHelpRequestAsync(labRequest);
                var result = _mapper.Map<HelpRequestDetailDto>(labRequest);
                return new HelpRequestDto { DetailsDto = result };
            }
        }
    }
}