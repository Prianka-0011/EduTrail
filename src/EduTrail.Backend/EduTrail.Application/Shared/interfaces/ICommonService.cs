using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace EduTrail.Application.Shared
{
    public interface ICommonService
    {
        IHttpContextAccessor _HttpContextAccessor { get; }
        ISchedulerFactory _SchedulerFactory { get; }
        IWebHostEnvironment _Environment { get; }
        IConfiguration _Configuration { get; }
        IJwtTokenGenerator _JwtTokenGenerator{ get; }
        IEmailTemplateService _EmailTemplateService  { get; }
        int SaltKeySize { get; }
        string _ApplicarionUrl { get; }
        string SmtpServer { get; }
        int _SmtpPort { get; }
        string _SmtpUsername { get; }
        string _SmtpPassowrd { get; }
        string _TimeZoneId { get; }
        bool _SmtpEnableSSL { get; }
        bool _SmtpEnabled { get; }
        string _SmtpNoReplyEmail { get; }
        string _SmtpNoReplyName { get; }
        string FilesDirectory { get; }
        string TempFilesDirectory { get; }
    }

}