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
                var currentLoginUserId = Guid.Parse("BE7CC9F8-7150-4E22-9137-08DE77468F3B");
                var labRequest = _mapper.Map<LabRequest>(request.HelpRequest);
                string prefix = "LR";
                var requestNumber = await _labRequestHelper.GenerateLabRequestNumber(prefix);
                labRequest.RequestNumber = requestNumber;
                labRequest.StudentId = currentLoginUserId;
                labRequest.StatusId = CustomCategory.HelpRequestStatus.Pending;
                await _repository.CreateHelpRequestAsync(labRequest);
                var result = _mapper.Map<HelpRequestDetailDto>(labRequest);
                return new HelpRequestDto { DetailsDto = result };
            }

           
        }
    }
}