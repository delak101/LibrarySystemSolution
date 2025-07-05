using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Services.Interfaces
{
    public interface ISignalRNotificationService
    {
        Task SendNotificationToUserAsync(int userId, NotificationDto notification);
        Task SendNotificationToUsersAsync(List<int> userIds, NotificationDto notification);
        Task SendBroadcastNotificationAsync(NotificationDto notification);
        Task SendBookDueReminderAsync(int userId, string bookTitle, DateTime dueDate);
        Task SendOverdueNotificationAsync(int userId, string bookTitle);
        Task SendBookAvailableNotificationAsync(int userId, string bookTitle);
        Task SendBorrowConfirmationAsync(int userId, string bookTitle, DateTime dueDate);
        Task SendReturnConfirmationAsync(int userId, string bookTitle);
    }
}
