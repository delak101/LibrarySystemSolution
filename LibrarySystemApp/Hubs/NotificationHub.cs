using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibrarySystemApp.Hubs
{
    // [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            
            _logger.LogInformation($"SignalR connection attempt - User ID: {userId}, User Name: {userName}, Connection ID: {Context.ConnectionId}");
            
            if (!string.IsNullOrEmpty(userId))
            {
                // Add user to their personal group
                var groupName = $"User_{userId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                _logger.LogInformation($"✅ User {userId} ({userName}) connected to SignalR and added to group '{groupName}' with connection ID: {Context.ConnectionId}");
            }
            else
            {
                _logger.LogWarning($"❌ User connected to SignalR but no User ID found in claims. Connection ID: {Context.ConnectionId}");
                
                // Log all available claims for debugging
                if (Context.User?.Claims != null)
                {
                    foreach (var claim in Context.User.Claims)
                    {
                        _logger.LogInformation($"Available claim: {claim.Type} = {claim.Value}");
                    }
                }
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
                _logger.LogInformation($"User {userId} disconnected from SignalR");
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Client can join specific groups if needed
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("JoinedGroup", groupName);
        }

        // Client can leave specific groups
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("LeftGroup", groupName);
        }

        // Mark notification as read
        public async Task MarkNotificationAsRead(int notificationId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"User {userId} marked notification {notificationId} as read");
            
            // You can implement logic here to update the notification status in database
            await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);
        }
    }
}
