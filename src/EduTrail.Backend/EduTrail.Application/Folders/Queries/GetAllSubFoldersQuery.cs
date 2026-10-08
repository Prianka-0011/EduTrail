using AutoMapper;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Folders
{
    public class GetAllSubFoldersQuery : IRequest<FolderDto>
    {
        public Guid? ParentId { get; set; }
        public class Handler : IRequestHandler<GetAllSubFoldersQuery, FolderDto>
        {
            private readonly IFolderRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IFolderRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<FolderDto> Handle(GetAllSubFoldersQuery request, CancellationToken cancellationToken)
            {
                var folderDtos = await _repository.GetAllByParentIdAsync(request.ParentId);
                var folderDetaiListDtos = _mapper.Map<List<FolderDetailsDto>>(folderDtos);
                return new FolderDto { DetailsListDto = folderDetaiListDtos };
            }
        }
    }
}