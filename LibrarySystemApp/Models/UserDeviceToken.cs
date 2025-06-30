using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models
{
    public class UserDeviceToken
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string DeviceToken { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? DeviceType { get; set; } // "android", "ios", "web"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public User User { get; set; } = null!;
    }
}
