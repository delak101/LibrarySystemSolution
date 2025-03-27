using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Repositories.Interfaces
{
    public interface IBorrowRepository
    {
        Task<Borrow> AddBorrowAsync(Borrow borrow);
        Task<bool> UpdateBorrowAsync(Borrow borrow);
        Task<Borrow?> GetBorrowByIdAsync(int borrowId);
        Task<List<BorrowDto>> GetPendingBorrowsAsync();
        Task<List<BorrowDto>> GetBorrowedBooksAsync();
        Task<List<BorrowDto>> GetOverdueBooksAsync();
        Task<List<BorrowDto>> GetUserBorrowHistoryAsync(int userId);
    }
}