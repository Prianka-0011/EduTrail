using EduTrail.Application.Posts;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EduTrail.API.Hubs
{
    public class PostDiscussionHub : Hub
    {
        private readonly IMediator _mediator;

        public PostDiscussionHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinPost(Guid postId)
        {
            var groupName = GetPostGroupName(postId);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                groupName);
        }

        public async Task LeavePost(Guid postId)
        {
            var groupName = GetPostGroupName(postId);

            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                groupName);
        }

        public async Task CreateDiscussion(
            CreatePostDiscussionCommand command)
        {
            if (command == null)
            {
                return;
            }

            var userIdClaim =
                Context.User?.FindFirst("sub")?.Value
                ?? Context.User?.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(
                userIdClaim,
                out var userId))
            {
                throw new HubException(
                    "User is not authenticated.");
            }

            command.CreatedById = userId;

            var result =
                await _mediator.Send(command);

            var groupName =
                GetPostGroupName(command.PostId);

            await Clients
                .Group(groupName)
                .SendAsync(
                    "DiscussionCreated",
                    result);
        }

        public async Task UpdateDiscussion(
            UpdatePostDiscussionCommand command)
        {
            if (command == null)
            {
                return;
            }

            var result =
                await _mediator.Send(command);

            var groupName =
                GetPostGroupName(command.PostId);

            await Clients
                .Group(groupName)
                .SendAsync(
                    "DiscussionUpdated",
                    result);
        }

        public async Task DeleteDiscussion(
            Guid postId,
            Guid discussionId)
        {
            var result =
                await _mediator.Send(
                    new DeletePostDiscussionCommand
                    {
                        Id = discussionId
                    });

            if (!result)
            {
                return;
            }

            var groupName =
                GetPostGroupName(postId);

            await Clients
                .Group(groupName)
                .SendAsync(
                    "DiscussionDeleted",
                    discussionId);
        }

        private static string GetPostGroupName(
            Guid postId)
        {
            return $"post:{postId}";
        }
    }
}