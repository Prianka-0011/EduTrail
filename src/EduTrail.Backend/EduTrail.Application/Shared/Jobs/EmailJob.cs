using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace EduTrail.Application.Shared
{
    public class EmailUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        private static readonly ILogger<EmailUser> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<EmailUser>();
        internal static void Deconstruct(out object CommonName, out object Email)
        {
            _logger.LogError("Deconstruct method not implemented in EmailUser.");
            throw new NotImplementedException();
        }
    }

    public class EmailJobData
    {
        public List<EmailUser> To { get; set; } = new();
        public List<EmailUser> CC { get; set; } = new();
        public List<EmailUser> Bcc { get; set; } = new();
        public string Subject { get; set; }
        public string Html { get; set; }
        public string Text { get; set; }

    }

    public class EmailJob : IJob
    {
        private const string _jobDataKey = "job-data";
        private readonly ILogger<EmailJob> _logger;
        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _smtpNoReplyMailAddress;
        ICommonService _service;

        public EmailJob(ILogger<EmailJob> logger, ICommonService service)
        {
            _logger = logger;
            _service = service;

            _smtpClient = new SmtpClient(_service.SmtpServer)
            {
                Port = _service._SmtpPort,
                Credentials = new NetworkCredential(_service._SmtpUsername, _service._SmtpPassowrd),
                EnableSsl = _service._SmtpEnableSSL,
            };

            _smtpNoReplyMailAddress = new MailAddress(_service._SmtpNoReplyEmail, _service._SmtpNoReplyName);
        }
        public static (IJobDetail, ITrigger) CreateJobAndTrigger(EmailJobData data)
        {
            var dataMap = new JobDataMap();
            dataMap.Put(_jobDataKey, data);

            var guid = Guid.NewGuid().ToString();

            var job = JobBuilder.Create<EmailJob>()
                .WithIdentity("job" + guid, nameof(EmailJob))
                .UsingJobData(dataMap)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger" + guid, nameof(EmailJob))
                .StartNow()
                .Build();
            return (job, trigger);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobData = (EmailJobData)context.MergedJobDataMap.Get(_jobDataKey);
            if (jobData is null)
            {
                return;
            }
            var (html, text) = _service._EmailTemplateService.BuildEmailLayout(
                subject: jobData.Subject,
                userName: jobData.To.Count == 1 ? jobData.To[0].FullName : null,
                midHtmlBody: jobData.Html,
                midTextBody: jobData.Text
            );

            var mailMessage = new MailMessage
            {
                From = _smtpNoReplyMailAddress,
                Subject = jobData.Subject,
                Body = html,
                IsBodyHtml = true
            };
            jobData.To.ForEach(x =>
                        {
                            if (x != null && x.Email != null && x.FullName != null)
                            {
                                mailMessage.To.Add(new MailAddress(x.Email, x.FullName));
                            }
                        });
            jobData.CC.ForEach(x =>
            {
                mailMessage.CC.Add(new MailAddress(x.Email, x.FullName));
            });

            jobData.Bcc.ForEach(x =>
            {
                mailMessage.Bcc.Add(new MailAddress(x.Email, x.FullName));
            });

            try
            {
                _logger.LogDebug("Sending Email: {jobData}", JsonConvert.SerializeObject(jobData));
                // if (_service._SmtpEnabled)
                // {
                //     _smtpClient.Send(mailMessage);
                // }
                _smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while sending email: {jobData}", JsonConvert.SerializeObject(jobData));
                throw new Exception("An error occured while sending email", ex);
            }

        }
    }
}