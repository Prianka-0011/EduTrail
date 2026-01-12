using AutoMapper;
using EduTrail.Application.Shared.Dtos;
using MediatR;

namespace EduTrail.Application.Questions
{
    public class GetQuestionByIdQuery : IRequest<QuestionDto>
    {
        public Guid Id { get; set; }
        public class Handler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
        {
            private readonly IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var questionDetailDto = _mapper.Map<QuestionDetailDto>(entity);
                var questionDto = new QuestionDto
                {
                    Types = (await _repository.GetAllQuestionType())
                            .Select(x => new DropdownItemDto
                            {
                                Id = x.Id,
                                Name = x.Name
                            })
                            .ToList(),
                    Assesments = (await _repository.GetAllAssessment())
                                .Select(x => new DropdownItemDto
                                {
                                    Id = x.Id,
                                    Name = x.Title
                                })
                                .ToList(),
                    Details = questionDetailDto
                };
                return questionDto;
            }
        }
    }
}