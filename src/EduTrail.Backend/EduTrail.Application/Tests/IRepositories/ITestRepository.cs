using EduTrail.Domain.Entities;
namespace EduTrail.Application.Tests
{
    public interface ITestRepository
    {
        public  Task<string> Create(Test test);
    }
}