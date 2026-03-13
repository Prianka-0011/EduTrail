using EduTrail.Application.Enrolements;
using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Auths
{
    public class SignDto
    {
        public string? Email { get; set; } = "";
        public string? Password { get; set; }
    }
}