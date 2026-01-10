namespace EduTrail.Application.Questions
{
    using MediatR;
    using EduTrail.Application.Questions;
    using EduTrail.Domain.Entities;
    using System.Text.Json;
    using AutoMapper;

    public class CreateQuestionCommand : IRequest<QuestionDto>
    {
        public QuestionDto QuestionDto { get; set; }
        public class Handler : IRequestHandler<CreateQuestionCommand, QuestionDto>
        {
            private readonly IQuestionRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    Title = request.QuestionDto.Title,
                    VariantTemplates = new List<QuestionVariantTemplate>(),
                    VariationRules = new List<QuestionVariationRule>()
                };

                question.VariantTemplates.Add(new QuestionVariantTemplate
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Template = request.QuestionDto.Template,
                    Language = request.QuestionDto.Language
                });

                if (request.QuestionDto.VariationRules != null)
                {
                    foreach (var ruleDto in request.QuestionDto.VariationRules)
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
                var responseDto = _mapper.Map<QuestionDto>(result);
                return responseDto;
            }
        }
    }
}