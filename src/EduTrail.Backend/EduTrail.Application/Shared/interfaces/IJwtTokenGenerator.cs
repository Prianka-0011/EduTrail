
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Shared
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, string tokenType = "");
    }

}