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
                .Include(b => b.User)  // Include User to get name and profile picture
                .Include(b => b.Book)  // Include Book to get title, shelf, and image
                    .ThenInclude(b => b!.Authors)  // Include Authors to get author names
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User!.Id,
                    StudentName = b.User!.Name,
                    StudentPfp = b.User!.ProfilePicture,
                    BookId = b.Book!.Id,
                    BookTitle = b.Book!.Name,
                    BookShelf = b.Book!.Shelf,
                    BookImg = b.Book!.Image,
                    BookAuthor = b.Book!.Authors != null && b.Book.Authors.Any() 
                        ? string.Join(", ", b.Book.Authors.Select(a => a.Name))
                        : null,
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
                    .ThenInclude(b => b!.Authors)
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User!.Id,
                    StudentName = b.User!.Name,
                    StudentPfp = b.User!.ProfilePicture,
                    BookId = b.Book!.Id,
                    BookTitle = b.Book!.Name,
                    BookShelf = b.Book!.Shelf,
                    BookImg = b.Book!.Image,
                    BookAuthor = b.Book!.Authors != null && b.Book.Authors.Any() 
                        ? string.Join(", ", b.Book.Authors.Select(a => a.Name))
                        : null,
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
                    .ThenInclude(b => b!.Authors)
                .Select(b => new BorrowDto()
                {
                    Id = b.Id,
                    StudentId = b.User!.Id,
                    StudentName = b.User!.Name,
                    StudentPfp = b.User!.ProfilePicture,
                    BookId = b.Book!.Id,
                    BookTitle = b.Book!.Name,
                    BookShelf = b.Book!.Shelf,
                    BookImg = b.Book!.Image,
                    BookAuthor = b.Book!.Authors != null && b.Book.Authors.Any() 
                        ? string.Join(", ", b.Book.Authors.Select(a => a.Name))
                        : null,
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
                .Include(b => b.User)
                .Include(b => b.Book)
                    .ThenInclude(book => book!.Authors)
                .Select(b => new BorrowDto
                {
                    Id = b.Id,
                    StudentId = b.User!.Id,
                    StudentPfp = b.User!.ProfilePicture,
                    StudentName = b.User!.Name,
                    BookId = b.Book!.Id,
                    BookImg = b.Book!.Image,
                    BookTitle = b.Book!.Name,
                    BookAuthor = b.Book!.Authors != null ? string.Join(", ", b.Book.Authors.Select(a => a.Name)) : "",
                    BookShelf = b.Book!.Shelf,
                    BorrowDate = b.BorrowDate,
                    DueDate = b.DueDate,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
        }
    }
}