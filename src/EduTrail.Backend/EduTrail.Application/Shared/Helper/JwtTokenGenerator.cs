using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduTrail.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EduTrail.Application.Shared
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly ICommonService _service;

        public JwtTokenGenerator(ICommonService service)
        {
            _service = service;
        }

        public string GenerateToken(User user, string tokenType = "")
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_service._Configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (!string.IsNullOrEmpty(tokenType))
            {
                claims.Add(new Claim("type", tokenType));
            }

            var token = new JwtSecurityToken(
                issuer: _service._Configuration["Jwt:Issuer"],
                audience: _service._Configuration["Jwt:Audience"],
                claims: claims,
                expires: tokenType == "reset-pass"
                    ? DateTime.UtcNow.AddMinutes(30)
                    : DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
