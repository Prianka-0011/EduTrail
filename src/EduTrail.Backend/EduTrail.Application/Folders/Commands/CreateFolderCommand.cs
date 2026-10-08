using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Folders
{
    public class CreateFolderCommand : IRequest<FolderDto>
    {
        public FolderDto FolderDto { get; set; } = new();

        public class Handler : IRequestHandler<CreateFolderCommand, FolderDto>
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

            public async Task<FolderDto> Handle(
                CreateFolderCommand request,
                CancellationToken cancellationToken)
            {
                var folders = _mapper.Map<List<Folder>>(
                    request.FolderDto.DetailsListDto);

                await _repository.CreateRangeAsync(folders);

                return new FolderDto
                {
                    DetailsListDto = _mapper.Map<List<FolderDetailsDto>>(folders)
                };
            }
        }
    }
}