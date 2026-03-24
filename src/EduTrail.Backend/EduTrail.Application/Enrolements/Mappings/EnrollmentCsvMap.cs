using CsvHelper.Configuration;

namespace EduTrail.Application.Enrolements
{
    public class EnrollmentCsvMap : ClassMap<EnrollmentCsvDto>
{
    public EnrollmentCsvMap()
    {
        Map(m => m.FullName).Name("Full name");
        Map(m => m.CanvasUserId).Name("Canvas user id");
        Map(m => m.Email).Name("Email");
        Map(m => m.SISId).Name("SIS Id");
    }
}
}