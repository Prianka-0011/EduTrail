using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EduTrail.Shared;

namespace EduTrail.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.Where(c => c.Id != CustomCategory.RoleType.TA).ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<DropdownItemDto> roleIds)
        {
            var roles = _context.Roles.Where(r => roleIds.Select(rId => rId.Id).Contains(r.Id)).AsEnumerable();
            return await Task.FromResult(roles);
        }
    }
}