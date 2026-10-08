using System.Collections;
using EduTrail.Application.CourseOfferings;
using EduTrail.Application.Courses;
using EduTrail.Application.Folders;
using EduTrail.Application.LabRequests;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using EduTrail.Shared;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly AppDbContext _context;
        public FolderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Folder>> CreateRangeAsync(List<Folder> folders)
        {
            await _context.Folders.AddRangeAsync(folders);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return folders;
        }
        public async Task<IEnumerable<Folder>> GetAllAsync(Guid? courseOfferingId = null)
        {
            var query = _context.Folders.AsQueryable();

            if (courseOfferingId.HasValue)
            {
                query = query.Where(c => c.CourseOfferingId == courseOfferingId.Value && c.ParentFolderId == null);
            }

            return await query
                .OrderBy(c => c.Name)
                .ThenBy(c => c.DisplayOrder)
                .ToListAsync();
        }
        public async Task<IEnumerable<Folder>> GetAllByParentIdAsync(Guid? parentId = null)
        {
            var subfolder = await _context.Folders.Include(c => c.ParentFolder).Where(c => c.ParentFolderId == parentId).ToListAsync();
            return subfolder;
        }
        public async Task<List<Folder>> GetByIdsAsync(List<Guid> folderIds)
        {
            return await _context.Folders.Include(c => c.SubFolders)
                .Where(x => folderIds.Contains(x.Id))
                .ToListAsync();
        }
        public async Task DeleteRangeAsync(List<Folder> folders)
        {
            _context.Folders.RemoveRange(folders);

            await _context.SaveChangesAsync();
        }

        public async Task<Folder?> GetByIdAsync(Guid id)
        {
            return await _context.Folders
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Folder> UpdateAsync(Folder folder)
        {
            _context.Folders.Update(folder);

            await _context.SaveChangesAsync();

            return folder;
        }
        public async Task<List<Folder>> GetSubByParentIdsAsync(List<Guid> parentIds)
        {
            return await _context.Folders
                .Where(f => f.ParentFolderId.HasValue &&
                            parentIds.Contains(f.ParentFolderId.Value))
                .ToListAsync();
        }
    }
}