using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services.Implementation;

public class BookService(IBookRepository _bookRepository, LibraryContext _context) : IBookService
{
        public async Task<BookResponseDto?> GetBookByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            return book is null ? null : MapToResponseDto(book);
        }

        public async Task<List<BookResponseDto>> GetBooksByNameAsync(string name)
        {
            var books = await _bookRepository.GetBooksByNameAsync(name);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksAsync()
        {
            var books = await _bookRepository.GetBooksAsync();
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<PagedResult<BookResponseDto>> GetBooksPagedAsync(int page, int pageSize)
        {
            var pagedBooks = await _bookRepository.GetBooksPagedAsync(page, pageSize);
            
            return new PagedResult<BookResponseDto>
            {
                Items = pagedBooks.Items.Select(MapToResponseDto).ToList(),
                TotalCount = pagedBooks.TotalCount,
                Page = pagedBooks.Page,
                PageSize = pagedBooks.PageSize
            };
        }

        public async Task<List<BookResponseDto>> GetBooksByGenreAsync(string genre)
        {
            var books = await _bookRepository.GetBooksByGenreAsync(genre);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByAuthorAsync(string author)
        {
            var books = await _bookRepository.GetBooksByAuthorAsync(author);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByAvailabilityAsync(bool isAvailable)
        {
            var books = await _bookRepository.GetBooksByAvailabilityAsync(isAvailable);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByYearAsync(int year)
        {
            var books = await _bookRepository.GetBooksByYearAsync(year);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<BookResponseDto>> GetBooksByDepartmentAsync(string department)
        {
            var books = await _bookRepository.GetBooksByDepartmentAsync(department);
            return books.Select(MapToResponseDto).ToList();
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            var authors = await _context.Authors.ToListAsync();
            return authors;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _bookRepository.GetCategoriesAsync();
            return categories;
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

            var addedBook = await _bookRepository.AddBookAsync(book);
            return MapToResponseDto(addedBook);
        }

        public async Task<BookResponseDto> UpdateBookAsync(int bookId, BookDto bookDto)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(bookId);
            if (existingBook is null) return null;

            // Only update fields if they are provided (not null)
            existingBook.Name = bookDto.Name ?? existingBook.Name;
            existingBook.Description = bookDto.Description ?? existingBook.Description;
            existingBook.Shelf = bookDto.Shelf ?? existingBook.Shelf;
            existingBook.IsAvailable = bookDto.IsAvailable;
            existingBook.Department = bookDto.Department ?? existingBook.Department;
            existingBook.AssignedYear = bookDto.AssignedYear ?? existingBook.AssignedYear;
            existingBook.Image = bookDto.Image ?? existingBook.Image;

            // Update Categories only if provided
            if (bookDto.CategoryNames != null && bookDto.CategoryNames.Any())
            {
                var categories = await _context.Categories
                    .Where(c => bookDto.CategoryNames.Contains(c.Name))
                    .ToListAsync();

                existingBook.Categories = categories;
            }

            // Update Authors only if provided
            if (bookDto.AuthorNames != null && bookDto.AuthorNames.Any())
            {
                var authors = await _context.Authors
                    .Where(a => bookDto.AuthorNames.Contains(a.Name))
                    .ToListAsync();

                existingBook.Authors = authors;
            }

            var updatedBook = await _bookRepository.UpdateBookAsync(existingBook);
            return MapToResponseDto(updatedBook);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books
                .Include(b => b.Borrows)
                .Include(b => b.Favorites)
                .Include(b => b.Categories)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null) return false; // Book not found

            // Remove related borrow records
            if (book.Borrows != null)
                _context.Borrows.RemoveRange(book.Borrows);

            // Remove related favorites
            if (book.Favorites != null)
                _context.Favorites.RemoveRange(book.Favorites);

            // Clear Many-to-Many relationships
            book.Categories.Clear();
            book.Authors.Clear();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
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
