using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;

namespace LibrarySystemApp.Services.Implementation;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<Book> AddBookAsync(BookDto bookDto)
    {
        var book = new Book
        {
            Name = bookDto.Name,
            Author = bookDto.Author,
            Description = bookDto.Description,
            Shelf = bookDto.Shelf,
            IsAvailable = bookDto.State,
            Department = bookDto.Department,
            AssignedYear= bookDto.Year
        };
        await bookRepository.AddBookAsync(book);

        return book;
    }

    public async Task<BookResponseDto> GetBookByIdAsync(int bookId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null) return null;

        // Manual mapping from Book to BookResponseDto
        return new BookResponseDto
        {
            Id = book.Id,
            Name = book.Name,
            Author = book.Author,
            Description = book.Description,
            Shelf = book.Shelf,
            State = book.IsAvailable,
            Department = book.Department,
            Year = book.AssignedYear
        };
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await bookRepository.GetBooksAsync();
        return books.Select(book => new BookResponseDto
        {
            Id = book.Id,
            Name = book.Name,
            Author = book.Author,
            Description = book.Description,
            Shelf = book.Shelf,
            State = book.IsAvailable,
            Department = book.Department,
            Year = book.AssignedYear
        }).ToList();
    }

    public async Task<BookResponseDto> UpdateBookAsync(int bookId, BookDto bookDto)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null) return null;

        // Update fields
        book.Name = bookDto.Name;
        book.Author = bookDto.Author;
        book.Description = bookDto.Description;
        book.Shelf = bookDto.Shelf;
        book.IsAvailable = bookDto.State;
        book.Department = bookDto.Department;
        book.AssignedYear = bookDto.Year;

        await bookRepository.UpdateBookAsync(book);

        // Return updated BookResponseDto
        return new BookResponseDto
        {
            Id = book.Id,
            Name = book.Name,
            Author = book.Author,
            Description = book.Description,
            Shelf = book.Shelf,
            State = book.IsAvailable,
            Department = book.Department,
            Year = book.AssignedYear
        };
    }

    public async Task<bool> DeleteBookAsync(int bookId)
    {
        var book = await bookRepository.GetBookByIdAsync(bookId);
        if (book == null) return false;

        await bookRepository.DeleteBookAsync(bookId);
        return true;
    }

    public List<Book> GetBooksByAuthor(string author)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetBooksByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetBooksByPublisher(string publisher)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetBooksByGenre(string genre)
    {
        throw new NotImplementedException();
    }
}