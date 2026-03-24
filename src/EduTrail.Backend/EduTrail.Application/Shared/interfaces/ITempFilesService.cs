using Microsoft.AspNetCore.Http;

public interface ITempFilesService
{
    Task<string> SaveTempFileAsync(IFormFile file);
    void DeleteTempFile(string path);
}