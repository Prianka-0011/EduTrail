using EduTrail.Domain.Entities;

namespace EduTrail.Application.Chats
{
    public interface IChatRepository
    {
       Task<ChatMessage> CreateAsync(ChatMessage message);
       Task<List<ChatMessage>> GetAllAsync();
    }
}