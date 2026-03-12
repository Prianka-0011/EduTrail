using EduTrail.Application.Auths;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.LabRequests
{
    public interface IAuthRepository
    {
        Task<User>GetUserByEmail(string email);
        Task<SignDto> SignIn();
    }
}