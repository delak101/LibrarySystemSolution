namespace LibrarySystemApp.DTOs
{
    public class RegisterTokenDto
    {
        public string DeviceToken { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
    }

    public class SendNotificationDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class SendBulkNotificationDto
    {
        public List<int> UserIds { get; set; } = new();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
