using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.Services;
using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Services.Implementation
{
    public class CombinedNotificationService : INotificationService
    {
        private readonly NotificationService _pushNotificationService;
        private readonly ISignalRNotificationService _signalRNotificationService;
        private readonly ILogger<CombinedNotificationService> _logger;

        public CombinedNotificationService(
            NotificationService pushNotificationService,
            ISignalRNotificationService signalRNotificationService,
            ILogger<CombinedNotificationService> logger)
        {
            _pushNotificationService = pushNotificationService;
            _signalRNotificationService = signalRNotificationService;
            _logger = logger;
        }

        public async Task<bool> SendNotificationAsync(int userId, string title, string body, object? data = null)
        {
            try
            {
                // Send both push notification and SignalR notification
                var pushTask = _pushNotificationService.SendNotificationAsync(userId, title, body, data);
                
                var signalRNotification = new NotificationDto
                {
                    Title = title,
                    Body = body,
                    Type = "general",
                    Data = data,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    UserId = userId
                };
                
                var signalRTask = _signalRNotificationService.SendNotificationToUserAsync(userId, signalRNotification);

                var results = await Task.WhenAll(pushTask, signalRTask.ContinueWith(t => true));
                return results[0]; // Return push notification result
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send combined notification to user {userId}");
                return false;
            }
        }

        public async Task<bool> SendNotificationToMultipleUsersAsync(List<int> userIds, string title, string body, object? data = null)
        {
            try
            {
                // Send both push notifications and SignalR notifications
                var pushTask = _pushNotificationService.SendNotificationToMultipleUsersAsync(userIds, title, body, data);
                
                var signalRNotification = new NotificationDto
                {
                    Title = title,
                    Body = body,
                    Type = "general",
                    Data = data,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                
                var signalRTask = _signalRNotificationService.SendNotificationToUsersAsync(userIds, signalRNotification);

                var results = await Task.WhenAll(pushTask, signalRTask.ContinueWith(t => true));
                return results[0]; // Return push notification result
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send combined notification to multiple users");
                return false;
            }
        }

        public async Task<bool> SendBookDueReminderAsync(int userId, string bookTitle, DateTime dueDate)
        {
            try
            {
                var pushTask = _pushNotificationService.SendBookDueReminderAsync(userId, bookTitle, dueDate);
                var signalRTask = _signalRNotificationService.SendBookDueReminderAsync(userId, bookTitle, dueDate);

                var results = await Task.WhenAll(pushTask, signalRTask.ContinueWith(t => true));
                return results[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send book due reminder to user {userId}");
                return false;
            }
        }

        public async Task<bool> SendOverdueNotificationAsync(int userId, string bookTitle)
        {
            try
            {
                var pushTask = _pushNotificationService.SendOverdueNotificationAsync(userId, bookTitle);
                var signalRTask = _signalRNotificationService.SendOverdueNotificationAsync(userId, bookTitle);

                var results = await Task.WhenAll(pushTask, signalRTask.ContinueWith(t => true));
                return results[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send overdue notification to user {userId}");
                return false;
            }
        }

        public async Task<bool> SendBookAvailableNotificationAsync(int userId, string bookTitle)
        {
            try
            {
                var pushTask = _pushNotificationService.SendBookAvailableNotificationAsync(userId, bookTitle);
                var signalRTask = _signalRNotificationService.SendBookAvailableNotificationAsync(userId, bookTitle);

                var results = await Task.WhenAll(pushTask, signalRTask.ContinueWith(t => true));
                return results[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send book available notification to user {userId}");
                return false;
            }
        }

        public async Task<bool> SendBorrowConfirmationAsync(int userId, string bookTitle, DateTime dueDate)
        {
            try
            {
                var signalRTask = _signalRNotificationService.SendBorrowConfirmationAsync(userId, bookTitle, dueDate);
                await signalRTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send borrow confirmation to user {userId}");
                return false;
            }
        }

        public async Task<bool> SendReturnConfirmationAsync(int userId, string bookTitle)
        {
            try
            {
                var signalRTask = _signalRNotificationService.SendReturnConfirmationAsync(userId, bookTitle);
                await signalRTask;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send return confirmation to user {userId}");
                return false;
            }
        }

        // Delegate methods for device token management
        public async Task<bool> RegisterDeviceTokenAsync(int userId, string deviceToken, string deviceType)
        {
            return await _pushNotificationService.RegisterDeviceTokenAsync(userId, deviceToken, deviceType);
        }

        public async Task<bool> RemoveDeviceTokenAsync(string deviceToken)
        {
            return await _pushNotificationService.RemoveDeviceTokenAsync(deviceToken);
        }

        public async Task<bool> SendUserApprovalNotificationAsync(int userId, string userName)
        {
            return await _pushNotificationService.SendUserApprovalNotificationAsync(userId, userName);
        }

        public async Task<bool> SendUserRejectionNotificationAsync(int userId, string userName)
        {
            return await _pushNotificationService.SendUserRejectionNotificationAsync(userId, userName);
        }
    }
}
