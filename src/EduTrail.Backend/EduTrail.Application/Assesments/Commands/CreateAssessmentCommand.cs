using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.QuestionTypes
{
    public class CreateQuestionTypeCommand : IRequest<QuestionTypeDetailDto>
    {
        public QuestionTypeDetailDto QuestionTypeDetailDto { get; set; }
        public class Handler : IRequestHandler<CreateQuestionTypeCommand, QuestionTypeDetailDto>
        {
            private readonly IQuestionTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<QuestionTypeDetailDto> Handle(CreateQuestionTypeCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<QuestionType>(request.QuestionTypeDetailDto);
                var result = await _repository.CreateAsync(entity);
                var questionTypeDto = _mapper.Map<QuestionTypeDetailDto>(result);
                return questionTypeDto;
            }
        }
    } 
}