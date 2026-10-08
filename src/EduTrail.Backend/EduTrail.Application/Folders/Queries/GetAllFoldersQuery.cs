using AutoMapper;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Folders
{
    public class GetAllFoldersQuery : IRequest<FolderDto>
    {
        public Guid? CourseOfferingId { get; set; }
        public class Handler : IRequestHandler<GetAllFoldersQuery, FolderDto>
        {
            private readonly IFolderRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IFolderRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<FolderDto> Handle(GetAllFoldersQuery request, CancellationToken cancellationToken)
            {
                var folderDtos = await _repository.GetAllAsync(request.CourseOfferingId);
                var folderDetaiListDtos = _mapper.Map<List<FolderDetailsDto>>(folderDtos);
                return new FolderDto { DetailsListDto = folderDetaiListDtos };
            }
        }
    }
}