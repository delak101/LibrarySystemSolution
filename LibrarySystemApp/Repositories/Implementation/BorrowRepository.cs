using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories
{
    public class BorrowRepository(LibraryContext _context) : IBorrowRepository
    {
        public async Task<Borrow> AddBorrowAsync(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();
            return borrow;
        }

        public async Task<bool> UpdateBorrowAsync(Borrow borrow)
        {
            _context.Borrows.Update(borrow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int borrowId)
        {
            return await _context.Borrows.FindAsync(borrowId);
        }

        public async Task<List<BorrowDto>> GetPendingBorrowsAsync()
        {
            return await _context.Borrows
                .Where(b => b.Status == BorrowStatus.Pending)
                .Include(b => b.User)  // Include User to get name
                .Include(b => b.Book)  // Include Book to get title
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User.Id,
                    StudentName = b.User.Name,
                    BookId = b.Book.Id,
                    BookTitle = b.Book.Name,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<BorrowDto>> GetBorrowedBooksAsync()
        {
            return await _context.Borrows
                .Where(b => b.Status == BorrowStatus.Approved)
                .Include(b => b.User)
                .Include(b => b.Book)
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User.Id,
                    StudentName = b.User.Name,
                    BookId = b.Book.Id,
                    BookTitle = b.Book.Name,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<BorrowDto>> GetOverdueBooksAsync()
        {
            return await _context.Borrows
                .Where(b => b.DueDate < DateTime.UtcNow && b.Status == BorrowStatus.Approved)
                .Include(b => b.User)
                .Include(b => b.Book)
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User.Id,
                    StudentName = b.User.Name,
                    BookId = b.Book.Id,
                    BookTitle = b.Book.Name,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<List<BorrowDto>> GetUserBorrowHistoryAsync(int userId)
        {
            return await _context.Borrows
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowDate)
                .Include(b => b.Book)
                .Select(b => new BorrowDto
                {
                    Id = b.Id,
                    StudentId = b.User.Id,
                    StudentName = b.User.Name,
                    BookId = b.Book.Id,
                    BookTitle = b.Book.Name,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }
    }
}