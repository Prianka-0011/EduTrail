using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Terms
{
    public class TermDto
    {
       public TermDetailDto DetailDto { get; set; }
       public List<TermDetailDto> DetailDtoList { get; set; }
       public List<DropdownItemDto> Types { get; set; } = new List<DropdownItemDto>();
    }
    public class TermDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Guid TermTypeId { get; set; }
        
    }
}