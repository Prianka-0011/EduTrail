using EduTrail.Application.Enrolements;
using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Auths
{
    public class ChangePasswordDto
    {
        public string? Token { get; set; } = "";
        public string? Password { get; set; }
    }
}