using AutoMapper;
using EduTrail.Application.Shared;
using EduTrail.Domain.Entities;
using MediatR;

namespace EduTrail.Application.Chats
{
    public class GetChatHistoryQuery : IRequest<List<ChatMessageDto>>
    {
        public Guid UserId { get; set; }       // Current user
        public Guid ReceiverId { get; set; }

        public class Handler : IRequestHandler<GetChatHistoryQuery, List<ChatMessageDto>>
        {
            private readonly ICommonService _service;
            private readonly IChatRepository _chatRepository;

            public Handler(ICommonService service, IChatRepository chatRepository)
            {
                _service = service;
                _chatRepository = chatRepository;
            }

            public async Task<List<ChatMessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
            {
                var messages = await _chatRepository
                    .GetAllAsync(); // You may want to filter by CourseOfferingId if needed

                return messages.Select(x =>
                {
                    string userName = string.IsNullOrWhiteSpace(x.User?.MiddleName)
                        ? $"{x.User?.FirstName} {x.User?.LastName}"
                        : $"{x.User?.FirstName} {x.User?.MiddleName} {x.User?.LastName}";

                    return new ChatMessageDto
                    {
                        UserId = x.UserId,
                        ReceiverId = x.ReceiverId,
                        CourseOfferingId = x.CourseOfferingId,
                        UserName = userName,
                        Message = x.Message,
                        CreatedDate = x.CreatedDate ?? DateTimeOffset.UtcNow
                    };
                }).ToList();
            }
        }
    }
}
