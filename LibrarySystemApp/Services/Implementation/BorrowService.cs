using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services
{
    public class BorrowService(IBorrowRepository _borrowRepository, LibraryContext _context) : IBorrowService
    {
        public async Task<Borrow> RequestBorrowAsync(int userId, int bookId, DateTime borrowDate, DateTime dueDate)
        {
            // var user = await _context.Users.FindAsync(userId);
            // if (user == null)
            //     throw new InvalidOperationException($"User with ID {userId} does not exist.");
            //
            // var book = await _context.Books.FindAsync(bookId);
            // if (book == null)
            //     throw new InvalidOperationException($"Book with ID {bookId} does not exist.");
            //
            // var isBookAlreadyBorrowed = await _context.Borrows
            // .AnyAsync(b => b.BookId == bookId && b.Status == BorrowStatus.Approved);
            // if (isBookAlreadyBorrowed)
            //     throw new InvalidOperationException($"Book with ID {bookId} is currently borrowed and not available.");

            var borrow = new Borrow
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = borrowDate,
                DueDate = borrowDate + TimeSpan.FromDays(7),
                Status = BorrowStatus.Pending,
            };

            return await _borrowRepository.AddBorrowAsync(borrow);
        }

        public async Task<bool> ApproveBorrowAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetBorrowByIdAsync(borrowId);
            if (borrow == null || borrow.Status != BorrowStatus.Pending)
                return false;

            var book = await _context.Books.FindAsync(borrow.BookId);
            if (book == null || !book.IsAvailable)
                return false;

            borrow.Status = BorrowStatus.Approved;
            book.IsAvailable = false;

            // Ensure both are updated successfully
            await _borrowRepository.UpdateBorrowAsync(borrow);
            await _context.SaveChangesAsync();
            return true; // Always return true if successful
        }

        public async Task<bool> DenyBorrowAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetBorrowByIdAsync(borrowId);
            if (borrow == null || borrow.Status != BorrowStatus.Pending)
                return false;

            borrow.Status = BorrowStatus.Denied;
            return await _borrowRepository.UpdateBorrowAsync(borrow);
        }

        public async Task<bool> ReturnBookAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetBorrowByIdAsync(borrowId);
            if (borrow == null || borrow.Status != BorrowStatus.Approved)
                return false;

            var book = await _context.Books.FindAsync(borrow.BookId);
            if (book == null)
                return false;

            borrow.Status = BorrowStatus.Returned;
            borrow.ReturnDate = DateTime.UtcNow;
            book.IsAvailable = true;

            // Ensure both are updated successfully
            await _borrowRepository.UpdateBorrowAsync(borrow);
            await _context.SaveChangesAsync();
            return true; // Always return true if successful
        }

        public async Task<List<BorrowDto>> GetPendingRequestsAsync()
        {
            return await _borrowRepository.GetPendingBorrowsAsync();
        }

        public async Task<List<BorrowDto>> GetBorrowedBooksAsync()
        {
            return await _borrowRepository.GetBorrowedBooksAsync();
        }

        public async Task<List<BorrowDto>> GetOverdueBooksAsync()
        {
            return await _borrowRepository.GetOverdueBooksAsync();
        }

        public async Task<List<BorrowDto>> GetUserBorrowHistoryAsync(int userId)
        {
            return await _borrowRepository.GetUserBorrowHistoryAsync(userId);
        }
    }
}
