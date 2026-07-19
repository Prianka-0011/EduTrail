using AutoMapper;
using MediatR;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Posts
{
    public class CreatePostCommand : IRequest<PostDto>
    {
        public PostDetailDto PostDetailDto { get; set; } = new();


        public class Handler : IRequestHandler<CreatePostCommand, PostDto>
        {
            private readonly IPostRepository _repository;
            private readonly IMapper _mapper;

            public Handler(
                IPostRepository repository,
                IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var dto = request.PostDetailDto;
                var post = _mapper.Map<Post>(dto);
                post.Id = Guid.NewGuid();

                // Attach folders
                if (dto.FolderIds != null && dto.FolderIds.Any())
                {
                    var folders = await _repository
                        .GetFoldersByIdsAsync(dto.FolderIds);


                    foreach (var folder in folders)
                    {
                        post.Folders.Add(folder);
                    }
                }

                // Attach enrollments
                if (dto.IsIndividual == true)
                {
                    if (dto.EnrollmentIds != null &&
                        dto.EnrollmentIds.Any())
                    {
                        var enrollments =
                            await _repository
                            .GetEnrollmentsByIdsAsync(dto.EnrollmentIds);


                        foreach (var enrollment in enrollments)
                        {
                            post.Enrollments.Add(enrollment);
                        }
                    }
                }
                else
                {
                    var courseOfferingId =
                        post.Folders
                            .Select(x => x.CourseOfferingId)
                            .FirstOrDefault();

                    if (courseOfferingId != Guid.Empty)
                    {
                        var enrollments =
                            await _repository
                            .GetEnrollmentsByCourseOfferingAsync(courseOfferingId);

                        foreach (var enrollment in enrollments)
                        {
                            post.Enrollments.Add(enrollment);
                        }
                    }
                }

                if (dto.Poll != null)
                {
                    post.Poll = new Poll
                    {
                        Id = Guid.NewGuid(),
                        Question = dto.Poll.Question,
                        Options = dto.Poll.Options
                            .Select(option => new PollOption
                            {
                                Id = Guid.NewGuid(),

                                OptionText = option.OptionText,

                                VoteCount = 0

                            })
                            .ToList()
                    };
                }

                var result = await _repository.CreateAsync(post);

                return new PostDto
                {
                    DetailsDto =
                        _mapper.Map<PostDetailDto>(result)
                };
            }
        }
    }
}