using LibrarySystemApp.DTOs;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpPost("add")] // POST: api/book/add
    public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
    {
        var addedBook = await bookService.AddBookAsync(bookDto);
        if (addedBook == null)
            return BadRequest("Failed to add the book.");

        return Ok("Book added successfully.");
    }

    [HttpGet("{bookId}")] // GET: api/book/{bookId}
    public async Task<ActionResult<BookResponseDto>> GetBookById(int bookId)
    {
        var book = await bookService.GetBookByIdAsync(bookId);
        if (book == null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpGet("search")] // GET: api/book/search?name=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByName([FromQuery] string name)
    {
        var books = await bookService.GetBooksByNameAsync(name);
        if (books == null || books.Count == 0)
            return NotFound("No books found with this name.");

        return Ok(books);
    }

    [HttpGet] // GET: api/book
    public async Task<ActionResult<List<BookResponseDto>>> GetAllBooks()
    {
        var books = await bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("genre")] // GET: api/book/genre?genre=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByGenre([FromQuery] string genre)
    {
        var books = await bookService.GetBooksByGenreAsync(genre);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this genre.");

        return Ok(books);
    }

    [HttpGet("author")] // GET: api/book/author?name=xyz
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByAuthor([FromQuery] string author)
    {
        var books = await bookService.GetBooksByAuthorAsync(author);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this author.");

        return Ok(books);
    }

    [HttpGet("year/{year}")] // GET: api/book/year/{year}
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByYear(int year)
    {
        var books = await bookService.GetBooksByYearAsync(year);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this year.");

        return Ok(books);
    }
    
    [HttpGet("available/{isAvailable}")]
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByAvailability(bool isAvailable)
    {
        var books = await bookService.GetBooksByAvailabilityAsync(isAvailable);
        if (books == null || books.Count == 0)
            return NotFound("No books available for borrowing.");

        return Ok(books);
    }

    [HttpGet("department/{department}")]
    public async Task<ActionResult<List<BookResponseDto>>> GetBooksByDepartment(string department)
    {
        var books = await bookService.GetBooksByDepartmentAsync(department);
        if (books == null || books.Count == 0)
            return NotFound("No books found for this department.");

        return Ok(books);
    }

    [HttpPut("update/{bookId}")] // PUT: api/book/update/{bookId}
    public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookDto bookDto)
    {
        var updatedBook = await bookService.UpdateBookAsync(bookId, bookDto);
        if (updatedBook == null)
            return NotFound("Book not found or update failed.");

        return Ok("Book updated successfully.");
    }

    [HttpDelete("delete/{bookId}")] // DELETE: api/book/delete/{bookId}
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        var result = await bookService.DeleteBookAsync(bookId);
        if (!result)
            return NotFound("Book not found.");

        return Ok("Book deleted successfully.");
    }
}