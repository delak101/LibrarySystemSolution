using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibrarySystemApp.Services;
using LibrarySystemApp.DTOs;
using System.Security.Claims;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterDeviceToken([FromBody] RegisterTokenDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0)
                {
                    return Unauthorized("Invalid user");
                }

                var result = await _notificationService.RegisterDeviceTokenAsync(
                    userId, dto.DeviceToken, dto.DeviceType);

                if (result)
                {
                    return Ok(new { message = "Device token registered successfully" });
                }

                return BadRequest(new { message = "Failed to register device token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RegisterDeviceToken");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
        {
            try
            {
                var result = await _notificationService.SendNotificationAsync(
                    dto.UserId, dto.Title, dto.Body, dto.Data);

                if (result)
                {
                    return Ok(new { message = "Notification sent successfully" });
                }

                return BadRequest(new { message = "Failed to send notification" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendNotification");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("send-bulk")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> SendBulkNotification([FromBody] SendBulkNotificationDto dto)
        {
            try
            {
                var result = await _notificationService.SendNotificationToMultipleUsersAsync(
                    dto.UserIds, dto.Title, dto.Body, dto.Data);

                if (result)
                {
                    return Ok(new { message = "Bulk notification sent successfully" });
                }

                return BadRequest(new { message = "Failed to send bulk notification" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendBulkNotification");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("remove-token")]
        public async Task<IActionResult> RemoveToken([FromBody] string deviceToken)
        {
            try
            {
                var result = await _notificationService.RemoveDeviceTokenAsync(deviceToken);
                
                if (result)
                {
                    return Ok(new { message = "Device token removed successfully" });
                }

                return BadRequest(new { message = "Failed to remove device token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveToken");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("test/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendTestNotification(int userId)
        {
            try
            {
                var result = await _notificationService.SendNotificationAsync(
                    userId, 
                    "Test Notification", 
                    "This is a test notification from FCI Library System!",
                    new { type = "test" });

                if (result)
                {
                    return Ok(new { message = "Test notification sent successfully" });
                }

                return BadRequest(new { message = "Failed to send test notification" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendTestNotification");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private int GetCurrentUserId()
        {
            try
            {
                // Try different claim types that might contain the user ID
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                               ?? User.FindFirst("UserId")?.Value 
                               ?? User.FindFirst("id")?.Value
                               ?? User.FindFirst("sub")?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    return userId;
                }

                _logger.LogWarning("Could not parse user ID from claims: {UserIdClaim}", userIdClaim);
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user ID");
                return 0;
            }
        }
    }
}
