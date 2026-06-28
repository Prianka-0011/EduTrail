using AutoMapper;
using MediatR;

namespace EduTrail.Application.Folders
{
    public class DeleteFoldersCommand : IRequest<FolderDto>
    {
        public List<Guid> FolderIds { get; set; } = new();

        public class Handler : IRequestHandler<DeleteFoldersCommand, FolderDto>
        {
            private readonly IFolderRepository _repository;
            private readonly IMapper _mapper;

            public Handler(
                IFolderRepository repository,
                IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<FolderDto> Handle(DeleteFoldersCommand request, CancellationToken cancellationToken)
            {
                if (request.FolderIds == null || !request.FolderIds.Any())
                {
                    return new FolderDto
                    {
                        IsSuccess = false,
                        DetailsListDto = new List<FolderDetailsDto>()
                    };
                }

                // Get selected parent folders
                var folders = await _repository.GetByIdsAsync(request.FolderIds);

                if (folders == null || !folders.Any())
                {
                    return new FolderDto
                    {
                        IsSuccess = false,
                        DetailsListDto = new List<FolderDetailsDto>()
                    };
                }

                // Delete child folders first
                var childFolders = await _repository.GetSubByParentIdsAsync(request.FolderIds);

                if (childFolders.Any())
                {
                    await _repository.DeleteRangeAsync(childFolders);
                }

                // Delete parent folders
                await _repository.DeleteRangeAsync(folders);

                return new FolderDto
                {
                    IsSuccess = true,
                    DetailsListDto = _mapper.Map<List<FolderDetailsDto>>(folders)
                };
            }
        }
    }
}