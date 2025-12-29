using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.Tests;
namespace EduTrail.Infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;
        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Create(Test entity)
        {
            var test = new Test ();
            test.Title = entity.Title;
            test.Description = entity.Description;
            _context.Add(test);
            await _context.SaveChangesAsync();
            return "Save Successfully";
        }
        
    }
}