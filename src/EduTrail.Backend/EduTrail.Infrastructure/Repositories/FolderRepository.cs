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

        public async Task<IEnumerable<Folder>> GetAllAsync(Guid? courseOfferingId = null)
        {
          return await  _context.Folders.Where(c=>c.CourseOfferingId == courseOfferingId).ToListAsync();
        }
    }
}