using EduTrail.Application.Shared.Dtos;
using EduTrail.Domain.Entities;

namespace EduTrail.Application.Users
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<DropdownItemDto> roleIds);
    }
}