namespace LibrarySystemApp.Services
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(int userId, string title, string body, object? data = null);
        Task<bool> SendNotificationToMultipleUsersAsync(List<int> userIds, string title, string body, object? data = null);
        Task<bool> RegisterDeviceTokenAsync(int userId, string deviceToken, string deviceType);
        Task<bool> RemoveDeviceTokenAsync(string deviceToken);
        Task<bool> SendBookDueReminderAsync(int userId, string bookTitle, DateTime dueDate);
        Task<bool> SendOverdueNotificationAsync(int userId, string bookTitle);
        Task<bool> SendBookAvailableNotificationAsync(int userId, string bookTitle);
        Task<bool> SendBorrowConfirmationAsync(int userId, string bookTitle, DateTime dueDate);
        Task<bool> SendReturnConfirmationAsync(int userId, string bookTitle);
    }
}
