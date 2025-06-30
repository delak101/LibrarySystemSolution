using LibrarySystemApp.Data;
using LibrarySystemApp.Models;
using LibrarySystemApp.Services;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(24); // Run daily

        public NotificationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessDueBookNotifications();
                    await ProcessOverdueBookNotifications();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in notification background service");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task ProcessDueBookNotifications()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            try
            {
                // Get books due in 1-2 days
                var tomorrow = DateTime.Today.AddDays(1);
                var dayAfterTomorrow = DateTime.Today.AddDays(2);

                var dueBooks = await context.Borrows
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .Where(b => b.Status == BorrowStatus.Approved && 
                               b.ReturnDate == null &&
                               b.DueDate >= tomorrow && 
                               b.DueDate <= dayAfterTomorrow)
                    .ToListAsync();

                foreach (var borrow in dueBooks)
                {
                    if (borrow.Book != null)
                    {
                        await notificationService.SendBookDueReminderAsync(
                            borrow.UserId,
                            borrow.Book.Name,
                            borrow.DueDate
                        );

                        _logger.LogInformation($"Sent due reminder for book '{borrow.Book.Name}' to user {borrow.UserId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing due book notifications");
            }
        }

        private async Task ProcessOverdueBookNotifications()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            try
            {
                // Get overdue books
                var today = DateTime.Today;
                var overdueBooks = await context.Borrows
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .Where(b => b.Status == BorrowStatus.Approved && 
                               b.ReturnDate == null &&
                               b.DueDate < today)
                    .ToListAsync();

                foreach (var borrow in overdueBooks)
                {
                    if (borrow.Book != null)
                    {
                        await notificationService.SendOverdueNotificationAsync(
                            borrow.UserId,
                            borrow.Book.Name
                        );

                        _logger.LogInformation($"Sent overdue notification for book '{borrow.Book.Name}' to user {borrow.UserId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing overdue book notifications");
            }
        }
    }
}
