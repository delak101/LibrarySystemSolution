using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services
{
    public class BorrowService(IBorrowRepository _borrowRepository, IUserRepository _userRepository, IBookRepository _bookRepository, LibraryContext _context) : IBorrowService
    {
        public async Task<BorrowDto> RequestBorrowAsync(int userId, int bookId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} does not exist.");
            
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
                throw new InvalidOperationException($"Book with ID {bookId} does not exist.");
            if (!book.IsAvailable)
                throw new InvalidOperationException($"Book with ID {bookId} is not available. already borrowed");
            
            var currentDate = DateTime.Today; // Use today's date
            
            var borrow = new Borrow
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = currentDate,
                DueDate = currentDate.AddDays(7), // 7 days from today
                Status = BorrowStatus.Pending,
            };

            var savedBorrow = await _borrowRepository.AddBorrowAsync(borrow);
            
            // Return a DTO to avoid circular references
            return new BorrowDto
            {
                Id = savedBorrow.Id,
                StudentId = savedBorrow.UserId,
                StudentName = user.Name,
                BookId = savedBorrow.BookId,
                BookTitle = book.Name,
                BookShelf = book.Shelf,
                BorrowDate = savedBorrow.BorrowDate,
                DueDate = savedBorrow.DueDate,
                Status = savedBorrow.Status.ToString()
            };
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
