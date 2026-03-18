using System.Globalization;
using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class SubmitHelpRequestCommand : IRequest<HelpRequestDto>
    {
        public HelpRequestDetailDto HelpRequest { get; set; }
        public class Handler : IRequestHandler<SubmitHelpRequestCommand, HelpRequestDto>
        {
            private readonly ILabRequestRepository _repository;
            private readonly IMapper _mapper;
            private readonly LabRequestHelper _labRequestHelper;

            public Handler(ILabRequestRepository repository, IMapper mapper,LabRequestHelper labRequestHelper )
            {
                _repository = repository;
                _mapper = mapper;
                _labRequestHelper = labRequestHelper;

            }
            public async Task<HelpRequestDto> Handle(SubmitHelpRequestCommand request, CancellationToken cancellationToken)
            {
                var currentLoginEnrollementUserId = Guid.Parse("6E0B0DF8-FCD8-465F-748A-08DE83B9AF28");
                var labRequest = _mapper.Map<LabRequest>(request.HelpRequest);
                string prefix = "LR";
                var requestNumber = await _labRequestHelper.GenerateLabRequestNumber(prefix);
                labRequest.RequestNumber = requestNumber;
                labRequest.StudentId = currentLoginEnrollementUserId;
                labRequest.StatusId = CustomCategory.HelpRequestStatus.Pending;
                await _repository.CreateHelpRequestAsync(labRequest);
                var result = _mapper.Map<HelpRequestDetailDto>(labRequest);
                return new HelpRequestDto { DetailsDto = result };
            }

           
        }
    }
}