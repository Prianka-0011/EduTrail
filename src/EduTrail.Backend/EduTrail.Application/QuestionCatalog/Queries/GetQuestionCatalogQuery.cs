using MediatR;

namespace EduTrail.Application.QuestionCatalog
{
    // Returns the list of questions discovered on disk. Read-only; no database involved.
    public class GetQuestionCatalogQuery : IRequest<List<QuestionCatalogItemDto>>
    {
        public class Handler : IRequestHandler<GetQuestionCatalogQuery, List<QuestionCatalogItemDto>>
        {
            private readonly IQuestionCatalogReader _reader;

            public Handler(IQuestionCatalogReader reader)
            {
                _reader = reader;
            }

            public Task<List<QuestionCatalogItemDto>> Handle(GetQuestionCatalogQuery request, CancellationToken cancellationToken)
            {
                return _reader.GetCatalogAsync();
            }
        }
    }
}
