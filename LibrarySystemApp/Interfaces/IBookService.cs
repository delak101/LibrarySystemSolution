using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Interfaces;

public interface IBookService
{
    Task<Book> AddBookAsync(BookDto bookDto);
    Task<BookResponseDto> GetBookByIdAsync(int bookId);
    Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto> UpdateBookAsync(int bookId, BookDto bookDto);
    Task<bool> DeleteBookAsync(int bookId);
    
    List<Book> GetBooksByAuthor(string author);
    List<Book> GetBooksByTitle(string title);
    List<Book> GetBooksByPublisher(string publisher);
    List<Book> GetBooksByGenre(string genre);
}