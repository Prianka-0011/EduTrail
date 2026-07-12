using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class GetPostByIdQuery : IRequest<PostDto>
    {
        public Guid Id { get; set; }
        public Guid CourseOfferingId { get; set; }

        public class Handler : IRequestHandler<GetPostByIdQuery, PostDto>
        {
            private readonly IPostRepository _postRepository;
            private readonly IMapper _mapper;

            public Handler(IPostRepository postRepository, IMapper mapper)
            {
                _postRepository = postRepository;
                _mapper = mapper;
            }

            public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                var post = await _postRepository.GetByIdAsync(request.Id);

                var postTypes = await _postRepository.GetAllTypeAsync();
                var postTypeDrop = postTypes?
                    .Select(c => new DropdownItemDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Desctiption = c.Description
                    })
                    .ToList() ?? new List<DropdownItemDto>();

                var enrollments = await _postRepository.GetEnrollmentsByCourseOfferingAsync(request.CourseOfferingId);
                var enrollmentDrop = enrollments?
                    .Select(c => new DropdownItemDto
                    {
                        Id = c.Id,
                        Name = $"{c.User?.FirstName} {c.User?.LastName}"
                    })
                    .ToList() ?? new List<DropdownItemDto>();

                var folders = await _postRepository.GetFoldersByCourseOfferingAsync(request.CourseOfferingId);

                var folderTree = BuildFolderTree(folders, null);

                if (post == null)
                {
                    return new PostDto
                    {
                        DetailsDto = new PostDetailDto
                        {
                            Id = Guid.Empty
                        },
                        Types = postTypeDrop,
                        Enrollements = enrollmentDrop,
                        Folders = folderTree
                    };
                }

                var postDetailsDto = _mapper.Map<PostDetailDto>(post);

                return new PostDto
                {
                    DetailsDto = postDetailsDto,
                    Types = postTypeDrop,
                    Enrollements = enrollmentDrop,
                    Folders = folderTree
                };
            }

            private List<FolderTree> BuildFolderTree(IEnumerable<Folder> folders, Guid? parentId)
            {
                return folders
                    .Where(f => f.ParentFolderId == parentId)
                    .Select(f => new FolderTree
                    {
                        Id = f.Id,
                        Name = f.Name,
                        ParentFolderId = f.ParentFolderId,
                        ChildFolders = BuildFolderTree(folders, f.Id)
                    })
                    .ToList();
            }
        }
    }
}