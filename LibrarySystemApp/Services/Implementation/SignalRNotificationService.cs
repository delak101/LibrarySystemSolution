using Microsoft.AspNetCore.SignalR;
using LibrarySystemApp.Hubs;
using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Services.Implementation
{
    public class SignalRNotificationService : ISignalRNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<SignalRNotificationService> _logger;

        public SignalRNotificationService(
            IHubContext<NotificationHub> hubContext,
            ILogger<SignalRNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationToUserAsync(int userId, NotificationDto notification)
        {
            try
            {
                var groupName = $"User_{userId}";
                
                _logger.LogInformation($"Sending SignalR notification to group '{groupName}' for user {userId}");
                
                await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", notification);
                
                _logger.LogInformation($"SignalR notification sent to user {userId}: {notification.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send SignalR notification to user {userId}");
            }
        }

        public async Task SendNotificationToUsersAsync(List<int> userIds, NotificationDto notification)
        {
            try
            {
                var groupNames = userIds.Select(id => $"User_{id}").ToList();
                foreach (var groupName in groupNames)
                {
                    await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", notification);
                }
                _logger.LogInformation($"SignalR notification sent to {userIds.Count} users: {notification.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send SignalR notification to multiple users");
            }
        }

        public async Task SendBroadcastNotificationAsync(NotificationDto notification)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
                _logger.LogInformation($"SignalR broadcast notification sent: {notification.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR broadcast notification");
            }
        }

        public async Task SendBookDueReminderAsync(int userId, string bookTitle, DateTime dueDate)
        {
            var notification = new NotificationDto
            {
                Title = "Book Due Reminder",
                Body = $"'{bookTitle}' is due on {dueDate:MMM dd, yyyy}",
                Type = "due_reminder",
                Data = new { BookTitle = bookTitle, DueDate = dueDate },
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = userId
            };

            await SendNotificationToUserAsync(userId, notification);
        }

        public async Task SendOverdueNotificationAsync(int userId, string bookTitle)
        {
            var notification = new NotificationDto
            {
                Title = "Overdue Book",
                Body = $"'{bookTitle}' is overdue. Please return it as soon as possible.",
                Type = "overdue",
                Data = new { BookTitle = bookTitle },
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = userId
            };

            await SendNotificationToUserAsync(userId, notification);
        }

        public async Task SendBookAvailableNotificationAsync(int userId, string bookTitle)
        {
            var notification = new NotificationDto
            {
                Title = "Book Available",
                Body = $"'{bookTitle}' is now available for borrowing!",
                Type = "book_available",
                Data = new { BookTitle = bookTitle },
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = userId
            };

            await SendNotificationToUserAsync(userId, notification);
        }

        public async Task SendBorrowConfirmationAsync(int userId, string bookTitle, DateTime dueDate)
        {
            var notification = new NotificationDto
            {
                Title = "Book Borrowed Successfully",
                Body = $"You have successfully borrowed '{bookTitle}'. Due date: {dueDate:MMM dd, yyyy}",
                Type = "borrow_confirmation",
                Data = new { BookTitle = bookTitle, DueDate = dueDate },
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = userId
            };

            await SendNotificationToUserAsync(userId, notification);
        }

        public async Task SendReturnConfirmationAsync(int userId, string bookTitle)
        {
            var notification = new NotificationDto
            {
                Title = "Book Returned Successfully",
                Body = $"You have successfully returned '{bookTitle}'. Thank you!",
                Type = "return_confirmation",
                Data = new { BookTitle = bookTitle },
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                UserId = userId
            };

            await SendNotificationToUserAsync(userId, notification);
        }
    }
}
