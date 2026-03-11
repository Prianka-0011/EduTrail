
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Shared
{
    public interface IConfigurationRepository
    {
        Task<AutoGenerateNumber> GetAutoGenerateNumberByPrefixAsync(string prefix);
        Task<AutoGenerateNumber> CreateAutoGenerate(AutoGenerateNumber autoGenerateNumber);
        Task<AutoGenerateNumber> UpdateAutoGenerate(AutoGenerateNumber autoGenerateNumber);
    }
}