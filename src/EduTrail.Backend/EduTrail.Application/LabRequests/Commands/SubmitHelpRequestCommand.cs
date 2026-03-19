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
            private readonly ICommonService _commonService;
            private readonly LabRequestHelper _labRequestHelper;


            public Handler(
                ILabRequestRepository repository,
                LabRequestHelper labRequestHelper,
                ICommonService commonService
                )
            {
                _repository = repository;
                _labRequestHelper = labRequestHelper;
                _commonService = commonService;

            }
            public async Task<HelpRequestDto> Handle(SubmitHelpRequestCommand request, CancellationToken cancellationToken)
            {
                var currentLoginUserId = _commonService._CurrentUserService.GetUserId();
                var enrolementId = 
                var labRequest = _commonService._Mapper.Map<LabRequest>(request.HelpRequest);
                string prefix = "LR";
                var requestNumber = await _labRequestHelper.GenerateLabRequestNumber(prefix);
                labRequest.RequestNumber = requestNumber;
                labRequest.StudentId = currentLoginEnrollementUserId;
                labRequest.StatusId = CustomCategory.HelpRequestStatus.Pending;
                await _repository.CreateHelpRequestAsync(labRequest);
                var result = _commonService._Mapper.Map<HelpRequestDetailDto>(labRequest);
                return new HelpRequestDto { DetailsDto = result };
            }


        }
    }
}