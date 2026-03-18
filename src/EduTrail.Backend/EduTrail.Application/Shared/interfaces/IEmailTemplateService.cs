namespace EduTrail.Application.Shared
{
    public interface IEmailTemplateService
    {
     (string html, string text)  PasswordResetTemplate(string verificationLink, string? userName);
       (string html, string text) BuildEmailLayout(string subject, string? userName, string midHtmlBody, string midTextBody);

    }

}
