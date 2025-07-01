using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsDto> GetAnalyticsAsync();
    }
}
