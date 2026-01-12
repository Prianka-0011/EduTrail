namespace EduTrail.Application.Questions
{
    using MediatR;
    using EduTrail.Application.Questions;
    using EduTrail.Domain.Entities;
    using System.Text.Json;
    using AutoMapper;

    public class CreateQuestionCommand : IRequest<QuestionDetailDto>
    {
        public QuestionDetailDto QuestionDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateQuestionCommand, QuestionDetailDto>
        {
            private readonly IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<QuestionDetailDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    Title = request.QuestionDetailDto.Title,
                    VariantTemplates = new List<QuestionVariantTemplate>(),
                    VariationRules = new List<QuestionVariationRule>()
                };

                question.VariantTemplates.Add(new QuestionVariantTemplate
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Template = request.QuestionDetailDto.Template,
                    Language = request.QuestionDetailDto.Language
                });

                if (request.QuestionDetailDto.VariationRules != null)
                {
                    foreach (var ruleDto in request.QuestionDetailDto.VariationRules)
                    {
                        question.VariationRules.Add(new QuestionVariationRule
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = question.Id,
                            Key = ruleDto.Key,
                            OptionsJson = JsonSerializer.Serialize(ruleDto.Options)
                        });
                    }
                }
                var result = await _repository.CreateAsync(question);
                var responseDto = _mapper.Map<QuestionDetailDto>(result);
                return responseDto;
            }
        }
    }
}