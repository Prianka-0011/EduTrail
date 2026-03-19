
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Auths
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> UpdateAsync(User user);
    }
}