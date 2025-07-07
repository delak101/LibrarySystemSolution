using LibrarySystemApp.Data;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibrarySystemApp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LibraryContext _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public NotificationService(
            LibraryContext context, 
            ILogger<NotificationService> logger,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterDeviceTokenAsync(int userId, string deviceToken, string deviceType)
        {
            try
            {
                // Check if token already exists
                var existingToken = await _context.UserDeviceTokens
                    .FirstOrDefaultAsync(t => t.DeviceToken == deviceToken);

                if (existingToken != null)
                {
                    // Update existing token
                    existingToken.UserId = userId;
                    existingToken.LastUpdated = DateTime.UtcNow;
                    existingToken.IsActive = true;
                    existingToken.DeviceType = deviceType;
                }
                else
                {
                    // Create new token record
                    _context.UserDeviceTokens.Add(new UserDeviceToken
                    {
                        UserId = userId,
                        DeviceToken = deviceToken,
                        DeviceType = deviceType
                    });
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Device token registered for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering device token for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> SendNotificationAsync(int userId, string title, string body, object? data = null)
        {
            try
            {
                // Get user's device tokens
                var deviceTokens = await _context.UserDeviceTokens
                    .Where(t => t.UserId == userId && t.IsActive)
                    .Select(t => t.DeviceToken)
                    .ToListAsync();

                if (!deviceTokens.Any())
                {
                    _logger.LogWarning($"No device tokens found for user {userId}");
                    return false;
                }

                return await SendFirebaseNotificationAsync(deviceTokens, title, body, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> SendNotificationToMultipleUsersAsync(List<int> userIds, string title, string body, object? data = null)
        {
            try
            {
                var deviceTokens = await _context.UserDeviceTokens
                    .Where(t => userIds.Contains(t.UserId) && t.IsActive)
                    .Select(t => t.DeviceToken)
                    .ToListAsync();

                if (!deviceTokens.Any())
                {
                    _logger.LogWarning($"No device tokens found for users: {string.Join(", ", userIds)}");
                    return false;
                }

                return await SendFirebaseNotificationAsync(deviceTokens, title, body, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notifications");
                return false;
            }
        }

        public async Task<bool> RemoveDeviceTokenAsync(string deviceToken)
        {
            try
            {
                var token = await _context.UserDeviceTokens
                    .FirstOrDefaultAsync(t => t.DeviceToken == deviceToken);

                if (token != null)
                {
                    token.IsActive = false;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Device token deactivated: {deviceToken[..10]}...");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing device token");
                return false;
            }
        }

        public async Task<bool> SendBookDueReminderAsync(int userId, string bookTitle, DateTime dueDate)
        {
            var daysUntilDue = (dueDate - DateTime.Now).Days;
            var message = daysUntilDue == 1 
                ? $"Your book '{bookTitle}' is due tomorrow!"
                : $"Your book '{bookTitle}' is due in {daysUntilDue} days.";

            return await SendNotificationAsync(
                userId,
                "Book Due Reminder",
                message,
                new { type = "due_reminder", bookTitle, dueDate }
            );
        }

        public async Task<bool> SendOverdueNotificationAsync(int userId, string bookTitle)
        {
            return await SendNotificationAsync(
                userId,
                "Overdue Book",
                $"Your book '{bookTitle}' is overdue. Please return it as soon as possible.",
                new { type = "overdue", bookTitle }
            );
        }

        public async Task<bool> SendBookAvailableNotificationAsync(int userId, string bookTitle)
        {
            return await SendNotificationAsync(
                userId,
                "Book Available",
                $"The book '{bookTitle}' is now available for borrowing!",
                new { type = "book_available", bookTitle }
            );
        }

        public async Task<bool> SendBorrowConfirmationAsync(int userId, string bookTitle, DateTime dueDate)
        {
            var title = "Book Borrowed Successfully";
            var body = $"You have successfully borrowed '{bookTitle}'. Due date: {dueDate:MMM dd, yyyy}";
            var data = new { BookTitle = bookTitle, DueDate = dueDate };
            
            return await SendNotificationAsync(userId, title, body, data);
        }

        public async Task<bool> SendReturnConfirmationAsync(int userId, string bookTitle)
        {
            var title = "Book Returned Successfully";
            var body = $"You have successfully returned '{bookTitle}'. Thank you!";
            var data = new { BookTitle = bookTitle };
            
            return await SendNotificationAsync(userId, title, body, data);
        }

        private async Task<bool> SendFirebaseNotificationAsync(List<string> deviceTokens, string title, string body, object? data)
        {
            try
            {
                // For now, we'll use a simple HTTP approach to Firebase FCM
                // You can later implement Firebase Admin SDK when you set up the service account
                
                var firebaseServerKey = _configuration["Firebase:ServerKey"];
                if (string.IsNullOrEmpty(firebaseServerKey))
                {
                    _logger.LogWarning("Firebase server key not configured. Notification not sent.");
                    return false;
                }

                var payload = new
                {
                    registration_ids = deviceTokens,
                    notification = new
                    {
                        title,
                        body
                    },
                    data = data ?? new { }
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"key={firebaseServerKey}");

                var response = await _httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully sent notification to {deviceTokens.Count} devices");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to send notification. Status: {response.StatusCode}, Error: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Firebase notification");
                return false;
            }
        }
    }
}
