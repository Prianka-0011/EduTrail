using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EduTrail.Application.Shared;
using EduTrail.Application.Auths;
using static EduTrail.Shared.CustomCategory;

namespace EduTrail.Application.Auths
{
    public class ChangePasswordCommand : IRequest<string>
    {
        public ChangePasswordDto ChangePasswordDto { get; set; }

        public class Handler : IRequestHandler<ChangePasswordCommand, string>
        {
            private readonly IAuthRepository _repository;
            private readonly ICommonService _commonService;

            public Handler(IAuthRepository repository, ICommonService commonService)
            {
                _repository = repository;
                _commonService = commonService;
            }

            public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var key = Encoding.UTF8.GetBytes(_commonService._Configuration["Jwt:Key"]);

                    var principal = tokenHandler.ValidateToken(
                        request.ChangePasswordDto.Token,
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _commonService._Configuration["Jwt:Issuer"],
                            ValidAudience = _commonService._Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ClockSkew = TimeSpan.Zero
                        },
                        out SecurityToken validatedToken
                    );

                    var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                    if (string.IsNullOrEmpty(email))
                        throw new Exception("Invalid token");

                    var type = principal.FindFirst("type")?.Value;
                    if (type != "reset-pass")
                        throw new Exception("Invalid token type");

                    var user = await _repository.GetUserByEmail(email);

                    if (user == null)
                        throw new Exception("User not found");
                    var context = _commonService._HttpContextAccessor.HttpContext;
                    if (context != null && context.Request.Cookies.ContainsKey(_commonService._AuthTokenCookieName))
                    {
                        context.Response.Cookies.Delete(_commonService._AuthTokenCookieName);
                    }
                    PasswordHasher.CreatePasswordHash(
                        request.ChangePasswordDto.Password,
                        out string hash,
                        out string salt
                    );

                    user.PasswordHash = hash;
                    user.PasswordSalt = salt;

                    await _repository.UpdateAsync(user);
                    return "Password changed successfully";
                }
                catch (SecurityTokenExpiredException)
                {
                    throw new Exception("Token expired");
                }
                catch
                {
                    throw new Exception("Invalid token");
                }
            }
        }
    }
}
