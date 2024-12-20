using LibrarySystemApp.Models;

namespace LibrarySystemApp.Repositories.Interfaces;

public interface IBookRepository
{
    // Retrieves a book by its ID
    Task<Book> GetBookByIdAsync(int bookId);

    // Retrieves a book by its name
    Task<Book> GetBookByNameAsync(string name);
    
    // Retrives all books
    Task<List<Book>> GetBooksAsync();

    // Adds a new book to the database
    Task AddBookAsync(Book book);

    // Updates an existing book's information
    Task UpdateBookAsync(Book book);

    // Deletes a book from the database by its ID
    Task DeleteBookAsync(int bookId);
}