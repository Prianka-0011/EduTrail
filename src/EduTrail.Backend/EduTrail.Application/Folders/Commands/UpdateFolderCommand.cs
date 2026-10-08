using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Folders
{
    public class UpdateFolderCommand : IRequest<FolderDetailsDto>
    {
        public FolderDetailsDto Folder { get; set; } = new();

        public class Handler : IRequestHandler<UpdateFolderCommand, FolderDetailsDto>
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

            public async Task<FolderDetailsDto> Handle(
                UpdateFolderCommand request,
                CancellationToken cancellationToken)
            {
                var existingFolder = await _repository.GetByIdAsync(request.Folder.Id);

                if (existingFolder == null)
                {
                    throw new Exception("Folder not found.");
                }

                existingFolder.Name = request.Folder.Name;
                existingFolder.DisplayOrder = request.Folder.DisplayOrder;
                existingFolder.ParentFolderId = request.Folder.ParentFolderId;
                existingFolder.CourseOfferingId = request.Folder.CourseOfferingId;

                var updatedFolder = await _repository.UpdateAsync(existingFolder);

                return _mapper.Map<FolderDetailsDto>(updatedFolder);
            }
        }
    }
}