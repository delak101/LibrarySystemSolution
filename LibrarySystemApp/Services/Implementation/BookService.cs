using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;

namespace LibrarySystemApp.Services.Implementation;

public class BookService(IBookRepository bookRepository) : IBookService
{

        public async Task<BookResponseDto?> GetBookByIdAsync(int bookId)
        {
            var book = await bookRepository.GetBookByIdAsync(bookId);
            return book is null ? null : MapToResponseDto(book);
        }

        public async Task<List<BookResponseDto>> GetBooksByNameAsync(string name)
        {
            var books = await bookRepository.GetBooksByNameAsync(name);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksAsync()
        {
            var books = await bookRepository.GetBooksAsync();
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByGenreAsync(string genre)
        {
            var books = await bookRepository.GetBooksByGenreAsync(genre);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByAuthorAsync(string author)
        {
            var books = await bookRepository.GetBooksByAuthorAsync(author);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByAvailabilityAsync(bool isAvailable)
        {
            var books = await bookRepository.GetBooksByAvailabilityAsync(isAvailable);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByYearAsync(int year)
        {
            var books = await bookRepository.GetBooksByYearAsync(year);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByDepartmentAsync(string department)
        {
            var books = await bookRepository.GetBooksByDepartmentAsync(department);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<BookResponseDto> AddBookAsync(BookDto bookDto)
        {
            var book = new Book
            {
                Name = bookDto.Name,
                Description = bookDto.Description,
                Shelf = bookDto.Shelf,
                IsAvailable = bookDto.IsAvailable,
                Department = bookDto.Department,
                AssignedYear = bookDto.AssignedYear,
                Image = bookDto.Image,
                Categories = bookDto.CategoryNames?.Select(name => new Category { Name = name }).ToList(),
                Authors = bookDto.AuthorNames?.Select(name => new Author { Name = name }).ToList()
            };

            var addedBook = await bookRepository.AddBookAsync(book);
            return MapToResponseDto(addedBook);
        }

        public async Task<BookResponseDto> UpdateBookAsync(int bookId, BookDto bookDto)
        {
            var existingBook = await bookRepository.GetBookByIdAsync(bookId);
            if (existingBook is null) return null;

            existingBook.Name = bookDto.Name;
            existingBook.Description = bookDto.Description;
            existingBook.Shelf = bookDto.Shelf;
            existingBook.IsAvailable = bookDto.IsAvailable;
            existingBook.Department = bookDto.Department;
            existingBook.AssignedYear = bookDto.AssignedYear;
            existingBook.Image = bookDto.Image;

            existingBook.Categories = bookDto.CategoryNames?.Select(name => new Category { Name = name }).ToList();
            existingBook.Authors = bookDto.AuthorNames?.Select(name => new Author { Name = name }).ToList();

            var updatedBook = await bookRepository.UpdateBookAsync(existingBook);
            return MapToResponseDto(updatedBook);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            return await bookRepository.DeleteBookAsync(bookId);
        }

        private static BookResponseDto MapToResponseDto(Book book) => new()
        {
            Id = book.Id,
            Name = book.Name,
            Description = book.Description,
            Shelf = book.Shelf,
            IsAvailable = book.IsAvailable,
            Department = book.Department,
            AssignedYear = book.AssignedYear,
            Image = book.Image,
            CategoryNames = book.Categories?.Select(c => c.Name).ToList(),
            AuthorNames = book.Authors?.Select(a => a.Name).ToList()
        };
}
