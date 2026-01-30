using AutoMapper;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.QuestionTypes
{
    public class UpdateQuestionTypeCommand : IRequest<QuestionTypeDetailDto>
    {
        public QuestionTypeDetailDto QuestionTypeDetailDto { get; set; }
        public class Handler : IRequestHandler<UpdateQuestionTypeCommand, QuestionTypeDetailDto>
        {
            private readonly IQuestionTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<QuestionTypeDetailDto> Handle(UpdateQuestionTypeCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<QuestionType>(request.QuestionTypeDetailDto);
                var result = await _repository.UpdateAsync(entity);
                var questionTypeDto = _mapper.Map<QuestionTypeDetailDto>(result);
                return questionTypeDto;
            }
        }
    }
}