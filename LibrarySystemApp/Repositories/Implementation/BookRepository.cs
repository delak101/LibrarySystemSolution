using LibrarySystemApp.Data;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories.Implementation;

public class BookRepository(LibraryContext context) : IBookRepository
{
    public async Task<Book?> GetBookByIdAsync(int bookId) =>
        await context.Books
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == bookId);

    public async Task<List<Book>> GetBooksByNameAsync(string name) =>
        await context.Books
            .Where(b => b.Name.Contains(name))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksAsync() =>
        await context.Books
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByGenreAsync(string genre) =>
        await context.Books
            .Where(b => b.Categories.Any(c => c.Name == genre))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByAuthorAsync(string author) =>
        await context.Books
            .Where(b => b.Authors.Any(a => a.Name == author))
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByAvailabilityAsync(bool isAvailable) =>
        await context.Books
            .Where(b => b.IsAvailable == isAvailable)
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<List<Book>> GetBooksByYearAsync(int year) =>
        await context.Books
            .Where(b => b.AssignedYear == year)
            .Include(b => b.Categories)
            .Include(b => b.Authors)
            .ToListAsync();

    public async Task<Book> AddBookAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(int bookId)
    {
        var book = await context.Books.FindAsync(bookId);
        if (book == null) return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }
}
