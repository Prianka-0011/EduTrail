using AutoMapper;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Posts
{
    public class UpdatePostCommand : IRequest<PostDto>
    {
        public PostDetailDto PostDetailDto { get; set; } = new();


        public class Handler : IRequestHandler<UpdatePostCommand, PostDto>
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


            public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var dto = request.PostDetailDto;


                var post = await _repository.GetByIdAsync(dto.Id);


                if (post == null)
                {
                    throw new Exception("Post not found.");
                }

                _mapper.Map(dto, post);

                post.Folders.Clear();
                if (dto.FolderIds != null && dto.FolderIds.Any())
                {
                    var folders =
                        await _repository.GetFoldersByIdsAsync(dto.FolderIds);


                    foreach (var folder in folders)
                    {
                        post.Folders.Add(folder);
                    }
                }

                post.Enrollments.Clear();


                if (dto.IsIndividual == true)
                {
                    if (dto.EnrollmentIds != null &&
                        dto.EnrollmentIds.Any())
                    {
                        var enrollments =
                            await _repository.GetEnrollmentsByIdsAsync(
                                dto.EnrollmentIds);


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
                                .GetEnrollmentsByCourseOfferingAsync(
                                    courseOfferingId);


                        foreach (var enrollment in enrollments)
                        {
                            post.Enrollments.Add(enrollment);
                        }
                    }
                }

                if (dto.PostTypeId == CustomCategory.PostTypes.Poll)
                {
                    // Existing post does not have poll, create new poll
                    if (post.Poll == null)
                    {
                        post.Poll = new Poll
                        {
                            
                            Question = dto.Poll.Question,
                            Options = dto.Poll.Options
                                .Select(option => new PollOption
                                {
                                    // Id = Guid.NewGuid(),
                                    OptionText = option.OptionText,
                                    VoteCount = 0
                                })
                                .ToList()
                        };
                    }
                    else
                    {
                        // Existing poll update
                        var existingOptions = post.Poll.Options.ToList();

                        var incomingIds = dto.Poll.Options
                            .Where(x => x.Id != Guid.Empty)
                            .Select(x => x.Id)
                            .ToHashSet();


                        // Remove deleted options
                        foreach (var option in existingOptions)
                        {
                            if (!incomingIds.Contains(option.Id))
                            {
                                _repository.RemovePollOption(option);
                            }
                        }


                        // Update existing and add new options
                        foreach (var optionDto in dto.Poll.Options)
                        {
                            if (optionDto.Id != Guid.Empty)
                            {
                                var existingOption =
                                    existingOptions.FirstOrDefault(x => x.Id == optionDto.Id);

                                if (existingOption != null)
                                {
                                    existingOption.OptionText = optionDto.OptionText;
                                }
                            }
                            else
                            {
                                post.Poll.Options.Add(new PollOption
                                {
                                    // Id = Guid.NewGuid(),
                                    PollId = post.Poll.Id,
                                    OptionText = optionDto.OptionText,
                                    VoteCount = 0
                                });
                            }
                        }

                        post.Poll.Question = dto.Poll.Question;
                    }
                }
                else
                {
                    // Remove poll if post type changed from Poll to another type
                    if (post.Poll != null)
                    {
                        foreach (var option in post.Poll.Options.ToList())
                        {
                            _repository.RemovePollOption(option);
                        }

                        _repository.RemovePoll(post.Poll);

                        post.Poll = null;
                    }
                }

                var result =
                    await _repository.UpdateAsync(post);



                return new PostDto
                {
                    DetailsDto =
                        _mapper.Map<PostDetailDto>(result)
                };
            }
        }
    }
}