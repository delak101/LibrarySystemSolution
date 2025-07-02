using LibrarySystemApp.Models;
using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Repositories.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int bookId);
    Task<List<Book>> GetBooksByNameAsync(string name);
    Task<List<Book>> GetBooksAsync();
    Task<PagedResult<Book>> GetBooksPagedAsync(int page, int pageSize);
    Task<int> GetBooksCountAsync();
    Task<List<Book>> GetBooksByGenreAsync(string genre);
    Task<PagedResult<Book>> GetBooksByCategoryPagedAsync(string category, int page, int pageSize);
    Task<List<Book>> GetBooksByAuthorAsync(string author);
    Task<List<Book>> GetBooksByAvailabilityAsync(bool isAvailable);
    Task<List<Book>> GetBooksByYearAsync(int year);
    Task<List<Book>> GetBooksByDepartmentAsync(string department);
    Task<Book> AddBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<List<Author>> GetAuthorsAsync();
    Task<List<Category>> GetCategoriesAsync();
    Task<bool> DeleteBookAsync(int bookId);
}