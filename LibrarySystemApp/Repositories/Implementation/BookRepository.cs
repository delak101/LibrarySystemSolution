using LibrarySystemApp.Data;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories.Implementation;

public class BookRepository(LibraryContext _context) : IBookRepository
{
    public async Task<Book?> GetBookByIdAsync(int bookId) =>
        await _context.Books
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == bookId);

    public async Task<List<Book>> GetBooksByNameAsync(string name) =>
        await _context.Books
            .Where(b => b.Name.ToLower().Contains(name.ToLower()))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksAsync() =>
        await _context.Books
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();
    

    public async Task<List<Book>> GetBooksByGenreAsync(string genre) =>
        await _context.Books
            .Where(b => b.Categories.Any(g => g.Name.ToLower().Contains(genre.ToLower())))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByAuthorAsync(string author) =>
        await _context.Books
            .Where(b => b.Authors.Any(a => a.Name.ToLower().Contains(author.ToLower())))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByAvailabilityAsync(bool isAvailable) =>
        await _context.Books
            .Where(b => b.IsAvailable == isAvailable)
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByYearAsync(int year) =>
        await _context.Books
            .Where(b => b.AssignedYear == year)
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByDepartmentAsync(string department) =>
        await _context.Books
            .Where(b => b.Department == department)
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Author>> GetAuthorsAsync() =>
        await _context.Authors
            .ToListAsync();
    
    public async Task<Book> AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
