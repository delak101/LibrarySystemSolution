using LibrarySystemApp.DTOs;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;

namespace LibrarySystemApp.Services.Implementation
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBorrowRepository _borrowRepository;

        public AnalyticsService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IBorrowRepository borrowRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowRepository = borrowRepository;
        }

        public async Task<AnalyticsDto> GetAnalyticsAsync()
        {
            // Get all the data in parallel for better performance
            var booksCountTask = _bookRepository.GetBooksCountAsync();
            var usersTask = _userRepository.GetAllUsersAsync();
            var pendingBorrowsTask = _borrowRepository.GetPendingBorrowsAsync();
            var borrowedBooksTask = _borrowRepository.GetBorrowedBooksAsync();

            await Task.WhenAll(booksCountTask, usersTask, pendingBorrowsTask, borrowedBooksTask);

            var booksCount = await booksCountTask;
            var users = await usersTask;
            var pendingBorrows = await pendingBorrowsTask;
            var borrowedBooks = await borrowedBooksTask;

            return new AnalyticsDto
            {
                TotalBooks = booksCount,
                TotalStudents = users?.Count ?? 0,
                PendingRequests = pendingBorrows?.Count ?? 0,
                BorrowedBooks = borrowedBooks?.Count ?? 0
            };
        }
    }
}
