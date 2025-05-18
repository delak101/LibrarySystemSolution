using LibrarySystemApp.Models;

namespace LibrarySystemApp.Repositories.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int bookId);
    Task<List<Book>> GetBooksByNameAsync(string name);
    Task<List<Book>> GetBooksAsync();
    Task<List<Book>> GetBooksByGenreAsync(string genre);
    Task<List<Book>> GetBooksByAuthorAsync(string author);
    Task<List<Book>> GetBooksByAvailabilityAsync(bool isAvailable);
    Task<List<Book>> GetBooksByYearAsync(int year);
    Task<List<Book>> GetBooksByDepartmentAsync(string department);
    Task<Book> AddBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<List<Author>> GetAuthorsAsync();
    Task<bool> DeleteBookAsync(int bookId);
}