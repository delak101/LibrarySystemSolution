using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.DTOs;
using System.Security.Claims;

namespace LibrarySystemApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class SignalRNotificationController : ControllerBase
    {
        private readonly ISignalRNotificationService _signalRNotificationService;
        private readonly ICombinedNotificationService _combinedNotificationService;
        private readonly ILogger<SignalRNotificationController> _logger;

        public SignalRNotificationController(
            ISignalRNotificationService signalRNotificationService,
            ICombinedNotificationService combinedNotificationService,
            ILogger<SignalRNotificationController> logger)
        {
            _signalRNotificationService = signalRNotificationService;
            _combinedNotificationService = combinedNotificationService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto request)
        {
            try
            {
                var notification = new NotificationDto
                {
                    Title = request.Title,
                    Body = request.Body,
                    Type = "general",
                    Data = request.Data,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    UserId = request.UserId
                };

                await _signalRNotificationService.SendNotificationToUserAsync(request.UserId, notification);
                return Ok(new { success = true, message = "SignalR notification sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR notification");
                return StatusCode(500, new { success = false, message = "Failed to send notification" });
            }
        }

        [HttpPost("send-bulk")]
        public async Task<IActionResult> SendBulkNotification([FromBody] SendBulkNotificationDto request)
        {
            try
            {
                var notification = new NotificationDto
                {
                    Title = request.Title,
                    Body = request.Body,
                    Type = "general",
                    Data = request.Data,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                await _signalRNotificationService.SendNotificationToUsersAsync(request.UserIds, notification);
                return Ok(new { success = true, message = "SignalR bulk notification sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR bulk notification");
                return StatusCode(500, new { success = false, message = "Failed to send bulk notification" });
            }
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> SendBroadcastNotification([FromBody] SendNotificationDto request)
        {
            try
            {
                var notification = new NotificationDto
                {
                    Title = request.Title,
                    Body = request.Body,
                    Type = "broadcast",
                    Data = request.Data,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                await _signalRNotificationService.SendBroadcastNotificationAsync(notification);
                return Ok(new { success = true, message = "SignalR broadcast notification sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR broadcast notification");
                return StatusCode(500, new { success = false, message = "Failed to send broadcast notification" });
            }
        }

        [HttpPost("combined/send")]
        public async Task<IActionResult> SendCombinedNotification([FromBody] SendNotificationDto request)
        {
            try
            {
                var result = await _combinedNotificationService.SendNotificationAsync(
                    request.UserId, 
                    request.Title, 
                    request.Body, 
                    request.Data);

                return Ok(new { success = result, message = result ? "Combined notification sent successfully" : "Failed to send combined notification" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send combined notification");
                return StatusCode(500, new { success = false, message = "Failed to send combined notification" });
            }
        }

        [HttpPost("combined/send-bulk")]
        public async Task<IActionResult> SendCombinedBulkNotification([FromBody] SendBulkNotificationDto request)
        {
            try
            {
                var result = await _combinedNotificationService.SendNotificationToMultipleUsersAsync(
                    request.UserIds, 
                    request.Title, 
                    request.Body, 
                    request.Data);

                return Ok(new { success = result, message = result ? "Combined bulk notification sent successfully" : "Failed to send combined bulk notification" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send combined bulk notification");
                return StatusCode(500, new { success = false, message = "Failed to send combined bulk notification" });
            }
        }

        [HttpGet("connection-test")]
        public IActionResult TestConnection()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { 
                success = true, 
                message = "SignalR controller is working", 
                userId = userId,
                timestamp = DateTime.UtcNow 
            });
        }
    }
}
