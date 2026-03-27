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
    public class ChangePasswordManuallyCommand : IRequest<string>
    {
        public ChangePasswordDto ChangePasswordDto { get; set; }

        public class Handler : IRequestHandler<ChangePasswordManuallyCommand, string>
        {
            private readonly IAuthRepository _repository;
            private readonly ICommonService _commonService;

            public Handler(IAuthRepository repository, ICommonService commonService)
            {
                _repository = repository;
                _commonService = commonService;
            }

            public async Task<string> Handle(ChangePasswordManuallyCommand request, CancellationToken cancellationToken)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    if (string.IsNullOrEmpty(request.ChangePasswordDto.Email))
                        throw new Exception("Invalid token");

                    var user = await _repository.GetUserByEmail(request.ChangePasswordDto.Email);

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
