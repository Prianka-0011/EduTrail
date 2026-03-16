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
    public class SignInCommand : IRequest<bool>
    {
        public SignDto Login { get; set; }

        public class Handler : IRequestHandler<SignInCommand, bool>
        {
            private readonly IAuthRepository _repository;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IWebHostEnvironment _environment;

            public Handler(
                IAuthRepository repository,
                IJwtTokenGenerator jwtTokenGenerator,
                IHttpContextAccessor httpContextAccessor,
                IWebHostEnvironment environment)
            {
                _repository = repository;
                _jwtTokenGenerator = jwtTokenGenerator;
                _httpContextAccessor = httpContextAccessor;
                _environment = environment;
            }

            public async Task<bool> Handle(SignInCommand request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUserByEmail(request.Login.Email);

                if (user == null || !user.IsActive)
                    throw new Exception("Invalid credentials");

                var isPasswordValid = PasswordHasher.VerifyPassword(
                    request.Login.Password,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                if (!isPasswordValid)
                    throw new Exception("Invalid credentials");

                var token = _jwtTokenGenerator.GenerateToken(user);

                var context = _httpContextAccessor.HttpContext;

                if (context != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = !_environment.IsDevelopment(),
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    context.Response.Cookies.Append(AuthsVariable.AuthTokenName, token, cookieOptions);
                }

                return true;
            }
        }
    }
}
