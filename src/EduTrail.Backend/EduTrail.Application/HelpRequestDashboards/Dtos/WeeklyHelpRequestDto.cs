namespace EduTrail.Application.HelpRequestDashboards
{
    public class WeeklyLabRequestDto
    {
        public List<WeeklyLabRequestDetailDto> WeeklyDataList { get; set; } = new List<WeeklyLabRequestDetailDto>();
    }

    public class WeeklyLabRequestDetailDto
    {
        public string Week { get; set; }
        public DateTime WeekStartDate { get; set; }

        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; set; }
        public int Total { get; set; }
    }
}