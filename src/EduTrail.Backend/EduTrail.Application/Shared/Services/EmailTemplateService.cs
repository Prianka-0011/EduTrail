namespace EduTrail.Application.Shared
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private const string PrimaryColor = "#368477";
        private const string ButtonText = "#ffffff";

        public (string html, string text) PasswordResetTemplate(string verificationLink, string? userName)
        {
            var htmlBody = GetPasswordResetBody(verificationLink);
            var textBody = $@"
                    A request has been received to reset your password.

                    Reset link:
                    {verificationLink}

                    If you did not request this, please ignore this email.
                    ";
            return (htmlBody, textBody);
        }

        private string GetPasswordResetBody(string verificationLink)
        {
            return $@"
                    <p>A request has been received to change your password.</p>

                    <p>
                        <a href='{verificationLink}'
                        style='background:{PrimaryColor};
                                color:{ButtonText};
                                padding:10px 16px;
                                text-decoration:none;
                                border-radius:6px;'>
                            Reset Password
                        </a>
                    </p>

                    <p>If you did not request this, please ignore this email.</p>
                    ";
        }

     public  (string html, string text) BuildEmailLayout(string subject, string? userName, string midHtmlBody, string midTextBody)
        {
            var greeting = string.IsNullOrEmpty(userName) ? "Hello," : $"Hello {userName},";
            
            var htmlBody = $@"
                    <html>
                    <body style='font-family:Arial,sans-serif;background:#f5f5f5;padding:20px;'>
                        <div style='max-width:600px;margin:auto;background:white;padding:20px;border-radius:8px;'>
                            <h2>{subject}</h2>
                            <p>{greeting}</p>
                            {midHtmlBody}
                            <p style='margin-top:20px'>
                                Thank you,<br/>
                                EduTrail Team
                            </p>
                        </div>
                    </body>
                    </html>
                    ";

            var textBody = $@"
                {subject}

                {greeting}

                {midTextBody}

                Thank you,
                EduTrail Team
                ";

            return (htmlBody, textBody);
        }
    }
}
