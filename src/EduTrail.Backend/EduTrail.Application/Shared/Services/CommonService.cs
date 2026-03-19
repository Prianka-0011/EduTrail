using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;

namespace EduTrail.Application.Shared
{
    public class CommonService : ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        // private readonly IMEdi
        private static readonly ILogger<CommonService> _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CommonService>();

        public CommonService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ISchedulerFactory schedulerFactory,
            IWebHostEnvironment environment,
            IEmailTemplateService emailTemplateService,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;
            _emailTemplateService = emailTemplateService;
            _environment = environment;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        private static string LogAndThrow(string message)
        {
            _logger.LogError(message);
            throw new Exception(message);
        }

        public string _AuthTokenCookieName => "Authorization";
        public ISchedulerFactory _SchedulerFactory => _schedulerFactory;
        public IEmailTemplateService _EmailTemplateService => _emailTemplateService;

        public int SaltKeySize => 64;
        public int HashIterations => 350000;
        public string _ApplicarionUrl => Environment.GetEnvironmentVariable("APPLICATION_URL") ?? "https://localhost:7238";
        public string _TimeZoneId => Environment.GetEnvironmentVariable("TIME_ZONE_ID") ?? "Central Standard Time";

        public string SmtpServer => Environment.GetEnvironmentVariable("SMTP_SERVER") ?? "smtp.gmail.com";
        // LogAndThrow("Environment variable 'SMTP_SERVER' must be set.");
        public int _SmtpPort => Convert.ToInt32(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
        public string _SmtpUsername => Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "priankanew0011@gmail.com";
        public string _SmtpPassowrd => Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "gdaz jwfh iwvb waqa" ;
        public bool _SmtpEnableSSL => (Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL") ?? ((_SmtpPort == 465 || _SmtpPort == 587) ? "true" : "false")) == "true";
        public bool _SmtpEnabled => (Environment.GetEnvironmentVariable("SMTP_ENABLED") ?? "false") == "true";
        public string _SmtpNoReplyEmail => Environment.GetEnvironmentVariable("SMTP_NO_REPLY_EMAIL") ?? "priankanew0011@gmail.com";
        public string _SmtpNoReplyName => Environment.GetEnvironmentVariable("SMTP_NO_REPLY_NAME") ?? "NoReplay";

        public string FilesDirectory => Environment.GetEnvironmentVariable("FILES_DIRECTORY") ?? System.IO.Path.Combine(Directory.GetCurrentDirectory(), "_files");
        public string TempFilesDirectory => Environment.GetEnvironmentVariable("TEMP_FILES_DIRECTORY") ?? System.IO.Path.Combine(Directory.GetCurrentDirectory(), "_temp-files");

        public IWebHostEnvironment _Environment => this._environment;

        public IConfiguration _Configuration =>  _configuration;

        public IJwtTokenGenerator _JwtTokenGenerator => _jwtTokenGenerator;
        public IMapper _Mapper => _mapper;
        public string _SecretKey => _configuration["Jwt:Key"];
        public ICurrentUserService _CurrentUserService => _currentUserService;
    }
}
