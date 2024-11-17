using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories;

public class BookRepository(LibraryContext context) : IBookRepository
{
    public async Task<Book> GetBookByIdAsync(int bookId) =>
        await context.Books.FindAsync(bookId);
    

    public async Task<Book> GetBookByNameAsync(string name) =>
        await context.Books.FirstOrDefaultAsync(b => b.Name == name);

    public async Task<List<Book>> GetBooksAsync() =>
        await context.Books.ToListAsync();
    

    public async Task AddBookAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int bookId)
    {
        var book = await context.Books.FindAsync(bookId);
        if (book != null)
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }
    }
}