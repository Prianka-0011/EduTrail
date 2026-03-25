using System.Globalization;
using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.LabRequests
{
    public class UpdateHelpRequestCommand : IRequest<HelpRequestDto>
    {
        public HelpRequestDetailDto HelpRequest { get; set; }

        public class Handler : IRequestHandler<UpdateHelpRequestCommand, HelpRequestDto>
        {
            private readonly ILabRequestRepository _repository;
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

            public async Task<HelpRequestDto> Handle(UpdateHelpRequestCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.HelpRequest == null)
                        throw new Exception("Help request data is required.");

                    var currentLoginUserId = _commonService._CurrentUserService.GetUserId();

                    if (currentLoginUserId == Guid.Empty)
                        throw new Exception("Invalid user.");

                    var enrolement = await _repository.GetEnrollementByUserId(currentLoginUserId);

                    if (enrolement == null)
                        throw new Exception("Enrollment not found for current user.");

                    var labRequest = _commonService._Mapper.Map<LabRequest>(request.HelpRequest);

                    if (labRequest == null)
                        throw new Exception("Failed to map help request.");

                    labRequest.AssignedTeacherId = enrolement.Id;
                    labRequest.AssignedTeacher = enrolement;

                    var created = await _repository.UpdateHelpRequestAsync(labRequest);

                    if (created == null)
                        throw new Exception("Failed to create help request.");

                    var result = _commonService._Mapper.Map<HelpRequestDetailDto>(created);

                    return new HelpRequestDto
                    {
                        DetailsDto = result
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error while creating help request: {ex.Message}");
                }
            }
        }
    }
}
