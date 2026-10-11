namespace EduTrail.Application.QuestionCatalog
{
    // Reads the on-disk question catalog (folders containing info.json / question.html / server.py).
    // Implemented in the Infrastructure layer so the Application layer stays free of file IO details.
    public interface IQuestionCatalogReader
    {
        Task<List<QuestionCatalogItemDto>> GetCatalogAsync();
    }
}
