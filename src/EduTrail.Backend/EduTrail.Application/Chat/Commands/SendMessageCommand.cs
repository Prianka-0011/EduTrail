using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Chats
{
    public class SendMessageCommand : IRequest<ChatMessageDto>
    {
        public ChatMessageDto DetailDto { get; set; }

        public class Handler : IRequestHandler<SendMessageCommand, ChatMessageDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IChatRepository _chatRepository;

            public Handler(IUserRepository userRepository, IChatRepository chatRepository)
            {
                _userRepository = userRepository;
                _chatRepository = chatRepository;
            }

            public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
            {
                // Get sender
                var sender = await _userRepository.GetByIdAsync(request.DetailDto.UserId);
                if (sender == null) throw new Exception("Sender not found");

                // Get receiver
                var receiver = await _userRepository.GetByIdAsync(request.DetailDto.ReceiverId);
                if (receiver == null) throw new Exception("Receiver not found");

                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    UserId = sender.Id,
                    ReceiverId = receiver.Id,
                    Message = request.DetailDto.Message,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                await _chatRepository.CreateAsync(chatMessage);

                // Compose sender name
                string userName = string.IsNullOrWhiteSpace(sender.MiddleName)
                    ? $"{sender.FirstName} {sender.LastName}"
                    : $"{sender.FirstName} {sender.MiddleName} {sender.LastName}";

                return new ChatMessageDto
                {
                    UserId = sender.Id,
                    ReceiverId = receiver.Id,
                    UserName = userName,
                    Message = chatMessage.Message,
                    CreatedDate = chatMessage.CreatedDate.Value
                };
            }
        }
    }
}
