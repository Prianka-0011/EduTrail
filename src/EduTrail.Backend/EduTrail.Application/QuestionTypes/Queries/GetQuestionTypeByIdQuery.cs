using AutoMapper;
using EduTrail.Application.CourseOfferings;
using MediatR;

namespace EduTrail.Application.QuestionTypes
{
    public class GetQuestionTypeByIdQuery : IRequest<QuestionTypeDetailDto>
    {
        public Guid Id {get; set;}
        public class Handler : IRequestHandler<GetQuestionTypeByIdQuery, QuestionTypeDetailDto>
        {
            private readonly IQuestionTypeRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IQuestionTypeRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<QuestionTypeDetailDto> Handle(GetQuestionTypeByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                var dto = _mapper.Map<QuestionTypeDetailDto>(entity);
                return dto;
            }
        }
    }
}