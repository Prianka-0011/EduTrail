using Microsoft.AspNetCore.Http;

namespace EduTrail.Application.Shared
{
    public class TempFilesService : ITempFilesService
    {
        private readonly string _tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles");

        public TempFilesService()
        {
            if (!Directory.Exists(_tempFolder))
                Directory.CreateDirectory(_tempFolder);
        }

        public async Task<string> SaveTempFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(_tempFolder, $"{Guid.NewGuid()}_{file.FileName}");
            using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            return filePath;
        }

        public void DeleteTempFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}