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
            private readonly ICommonService _service;
            public Handler(
                IAuthRepository repository,
                ICommonService service
                )
            {
                _repository = repository;
                _service = service;
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

                var token = _service._JwtTokenGenerator.GenerateToken(user);

                var context = _service._HttpContextAccessor.HttpContext;

                if (context != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = !_environment.IsDevelopment(),
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    context.Response.Cookies.Append(_service._AuthTokenCookieName, token, cookieOptions);
                }

                return true;
            }
        }
    }
}
