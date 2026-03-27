using Microsoft.AspNetCore.SignalR;
using EduTrail.Application.Chats;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace EduTrail.API.Hubs
{
    // [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private static readonly Dictionary<Guid, string> _connections = new();

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            var userIdClaim = Context.User?.FindFirst("sub")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                _connections[userId] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdClaim = Context.User?.FindFirst("sub")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                _connections.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(ChatMessageDto messageDto)
        {
            if (messageDto == null) return;

            var command = new SendMessageCommand
            {
                DetailDto = messageDto
            };

            var result = await _mediator.Send(command);

            if (_connections.TryGetValue(result.ReceiverId, out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", result);
            }

            if (_connections.TryGetValue(result.UserId, out var senderConnectionId))
            {
                await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", result);
            }
        }

        public async Task LoadHistory(Guid receiverId)
        {
            var userIdClaim = Context.User?.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId)) return;

            var messages = await _mediator.Send(new GetChatHistoryQuery
            {
                UserId = userId,
                ReceiverId = receiverId
            });

            await Clients.Caller.SendAsync("ReceiveHistory", messages);
        }
    }
}
