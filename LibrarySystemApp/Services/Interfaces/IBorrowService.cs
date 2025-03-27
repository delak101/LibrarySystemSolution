using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<Borrow> RequestBorrowAsync(int userId, int bookId, DateTime borrowDate, DateTime dueDate);
        Task<bool> ApproveBorrowAsync(int borrowId);
        Task<bool> DenyBorrowAsync(int borrowId);
        Task<bool> ReturnBookAsync(int borrowId);
        Task<List<BorrowDto>> GetPendingRequestsAsync();
        Task<List<BorrowDto>> GetBorrowedBooksAsync();
        Task<List<BorrowDto>> GetOverdueBooksAsync();
        Task<List<BorrowDto>> GetUserBorrowHistoryAsync(int userId);
    }
}