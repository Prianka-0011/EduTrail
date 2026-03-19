using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using EduTrail.Domain.Entities;
using EduTrail.Application.Shared;
using EduTrail.Application.LabRequests;
using static EduTrail.Shared.CustomCategory;

namespace EduTrail.Application.Auths
{
    public class ResetEmailCommand : IRequest<bool>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<ResetEmailCommand, bool>
        {
            private readonly IAuthRepository _repository;
            private readonly ICommonService _service;

            public Handler(
                IAuthRepository repository,
                ICommonService service)
            {
                _repository = repository;
               _service = service;
            }

            public async Task<bool> Handle(ResetEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUserByEmail(request.Email);

                if (user == null || !user.IsActive)
                    throw new Exception("User not found");

                var token = _service._JwtTokenGenerator.GenerateToken(user, "reset-pass");
                var baseUrl = Environment.GetEnvironmentVariable("APPLICATION_URL")?? "https://localhost:7238/";
                var resetLink = $"{baseUrl}change-password?token={token}";

                var (htmlBody, textBody) =
                _service._EmailTemplateService.PasswordResetTemplate(
                    resetLink,
                    user.FirstName + " " + user.LastName
                );

                var (job, trigger) = EmailJob.CreateJobAndTrigger(new EmailJobData
                {
                    To = new List<EmailUser> {

                   new() { FullName = user.FirstName+" "+user.LastName, Email = user.Email},
                },
                    Subject = "Reset Password",
                    Html = htmlBody,
                    Text = textBody
                });

                var scheduler = await _service._SchedulerFactory.GetScheduler();
                await scheduler.ScheduleJob(job, trigger, default);

                return true;
            }
        }
    }
}