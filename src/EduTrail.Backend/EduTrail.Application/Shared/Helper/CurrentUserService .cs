using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EduTrail.Application.Shared
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ICommonService _service;

        public CurrentUserService(ICommonService service)
        {
            _service = service;
        }

        public Guid GetUserId()
        {
            var context = _service.HttpContextAccessor.HttpContext;
            if (context == null || !context.Request.Cookies.TryGetValue(_service.AuthTokenCookieName, out var token))
                throw new UnauthorizedAccessException("User is not logged in");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_service.JwtTokenGenerator._SecretKey);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? throw new UnauthorizedAccessException("User ID claim not found");

            return Guid.Parse(userIdClaim);
        }
    }
}
