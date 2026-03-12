using EduTrail.Domain.Entities;

namespace EduTrail.Application.Shared
{
    public class LabRequestHelper
    {
        private readonly IConfigurationRepository _repository;

        public LabRequestHelper(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GenerateLabRequestNumber(string prefix)
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            int sequence = await GetNextSequenceForToday(prefix);
            return $"{prefix}-{datePart}-{sequence:D4}";
        }

        private async Task<int> GetNextSequenceForToday(string prefix)
        {
            var autoNum = await _repository.GetAutoGenerateNumberByPrefixAsync(prefix);

            if (autoNum == null)
            {
                autoNum = new AutoGenerateNumber
                {
                    Year = DateTime.UtcNow.Year,
                    Number = 1,
                    Prefix = prefix
                };

                autoNum = await _repository.CreateAutoGenerate(autoNum);
            }
            else
            {
                autoNum.Number = autoNum.Number + 1;
                autoNum = await _repository.UpdateAutoGenerate(autoNum);
            }

            return autoNum.Number;
        }
    }
}
