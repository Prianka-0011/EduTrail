using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using EduTrail.Domain.Entities;
using EduTrail.Application.Shared;
using EduTrail.Application.LabRequests;
using static EduTrail.Shared.CustomCategory;
using Microsoft.Extensions.Logging;

namespace EduTrail.Application.Auths
{
    public class SignInCommand : IRequest<bool>
    {
        public SignDto Login { get; set; }

        public class Handler : IRequestHandler<SignInCommand, bool>
        {
            private readonly IAuthRepository _repository;
            private readonly ICommonService _service;
            private readonly ILogger<Handler> _logger;
             private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(
                IAuthRepository repository,
                ICommonService service,
                 ILogger<Handler> logger,
                 IHttpContextAccessor httpContextAccessor
                )
            {
                _repository = repository;
                _service = service;
                _logger = logger;
                 _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(SignInCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Sign-in attempt for email: {Email}", request.Login.Email);

                    var user = await _repository.GetUserByEmail(request.Login.Email);

                    if (user == null || !user.IsActive)
                    {
                        _logger.LogWarning("Invalid login attempt: user not found or inactive for {Email}", request.Login.Email);
                        throw new Exception("Invalid credentials");
                    }

                    var isPasswordValid = PasswordHasher.VerifyPassword(
                        request.Login.Password,
                        user.PasswordHash,
                        user.PasswordSalt
                    );

                    if (!isPasswordValid)
                    {
                        _logger.LogWarning("Invalid password attempt for {Email}", request.Login.Email);
                        throw new Exception("Invalid credentials");
                    }

                    var token = _service._JwtTokenGenerator.GenerateToken(user);

                    var context = _httpContextAccessor.HttpContext;

                    if (context != null)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                            Path = "/"
                        };

                        context.Response.Cookies.Append(_service._AuthTokenCookieName, token, cookieOptions);
                    }

                    _logger.LogInformation("User signed in successfully: {Email}", request.Login.Email);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during sign-in for {Email}", request.Login.Email);
                    throw;
                }
            }
        }
    }
}
