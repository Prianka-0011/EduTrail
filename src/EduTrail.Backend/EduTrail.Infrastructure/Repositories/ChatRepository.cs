using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EduTrail.Infrastructure.Data;
using EduTrail.Domain.Entities;
using EduTrail.Application.Courses;
using EduTrail.Application.Assessments;
using EduTrail.Application.LabRequests;
using EduTrail.Application.Auths;
using EduTrail.Application.Chats;

namespace EduTrail.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ChatMessage> CreateAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<List<ChatMessage>> GetAllAsync()
        {
            return await _context.ChatMessages.ToListAsync();
        }
    }
}