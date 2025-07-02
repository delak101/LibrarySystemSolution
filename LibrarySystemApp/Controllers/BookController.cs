using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService _bookService) : ControllerBase
{
    [HttpPost("add")] // POST: api/book/add
    public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
    {
        var addedBook = await _bookService.AddBookAsync(bookDto);
        if (addedBook == null)
            return BadRequest("Failed to add the book.");

        return Ok("Book added successfully.");
    }

    [HttpGet("{bookId}")] // GET: api/book/{bookId}
    public async Task<ActionResult<BookResponseDto>> GetBookById(int bookId)
    {
        var book = await _bookService.GetBookByIdAsync(bookId);
        if (book == null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpGet("search")] // GET: api/book/search?name=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByName([FromQuery] string name)
    {
        var books = await _bookService.GetBooksByNameAsync(name);
        if (books == null || books.Count == 0)
            return NotFound("No books found with this name.");

        return Ok(books);
    }

    [HttpGet] // GET: api/book
    public async Task<ActionResult<List<BookResponseDto>>> GetAllBooks()
    {
        var books = await _bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("paged")] // GET: api/book/paged?page=1&pageSize=10
    public async Task<ActionResult<PagedResult<BookResponseDto>>> GetBooksPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50; // Limit maximum page size
        
        var pagedBooks = await _bookService.GetBooksPagedAsync(page, pageSize);
        return Ok(pagedBooks);
    }

    [HttpGet("category/paged")] // GET: api/book/category/paged?category=fiction&page=1&pageSize=10
    public async Task<ActionResult<PagedResult<BookResponseDto>>> GetBooksByCategoryPaged([FromQuery] string category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(category))
            return BadRequest("Category parameter is required.");
            
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50; // Limit maximum page size
        
        var pagedBooks = await _bookService.GetBooksByCategoryPagedAsync(category, page, pageSize);
        
        if (pagedBooks.Items.Count == 0)
            return NotFound($"No books found for category '{category}'.");
            
        return Ok(pagedBooks);
    }

    [HttpGet("genre")] // GET: api/book/genre?genre=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByGenre([FromQuery] string genre)
    {
        var books = await _bookService.GetBooksByGenreAsync(genre);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this genre.");

        return Ok(books);
    }

    [HttpGet("author")] // GET: api/book/author?name=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByAuthor([FromQuery] string author)
    {
        var books = await _bookService.GetBooksByAuthorAsync(author);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this author.");

        return Ok(books);
    }

    [HttpGet("year/{year}")] // GET: api/book/year/{year}
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByYear(int year)
    {
        var books = await _bookService.GetBooksByYearAsync(year);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this year.");

        return Ok(books);
    }
    
    [HttpGet("available/{isAvailable}")]
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByAvailability(bool isAvailable)
    {
        var books = await _bookService.GetBooksByAvailabilityAsync(isAvailable);
        if (books == null || books.Count == 0)
            return NotFound("No books available for borrowing.");

        return Ok(books);
    }

    [HttpGet("department/{department}")]
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByDepartment(string department)
    {
        var books = await _bookService.GetBooksByDepartmentAsync(department);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this department.");

        return Ok(books);
    }

    [HttpGet("authors")] // GET: api/book/authors get all authors
    public async Task<ActionResult<List<Author>>> GetAllAuthors()
    {
        var authors = await _bookService.GetAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("categories")] // GET: api/book/categories get all categories
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
        var categories = await _bookService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPut("update/{bookId}")] // PUT: api/book/update/{bookId}
    public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookDto bookDto)
    {
        var updatedBook = await _bookService.UpdateBookAsync(bookId, bookDto);
        if (updatedBook == null)
            return NotFound("Book not found or update failed.");

        return Ok("Book updated successfully.");
    }

    [HttpDelete("delete/{bookId}")] // DELETE: api/book/delete/{bookId}
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        var result = await _bookService.DeleteBookAsync(bookId);
        if (!result)
            return NotFound("Book not found.");

        return Ok("Book deleted successfully.");
    }
}