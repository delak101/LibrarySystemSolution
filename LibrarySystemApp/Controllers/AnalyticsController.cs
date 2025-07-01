using LibrarySystemApp.DTOs;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] 
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public async Task<ActionResult<AnalyticsDto>> GetAnalytics()
        {
            try
            {
                var analytics = await _analyticsService.GetAnalyticsAsync();
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching analytics data", details = ex.Message });
            }
        }
    }
}
