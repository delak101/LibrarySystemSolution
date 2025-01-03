﻿using LibrarySystemApp.DTOs;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpPost("addBook")] // POST: api/book/addBook
    public async Task<IActionResult> AddBook(BookDto bookDto)
    {
        var addedBook = await bookService.AddBookAsync(bookDto);
        if (addedBook == null)
            return BadRequest("Failed to add the book.");

        return Ok("Book Added Successfully");
    }
    
    [HttpGet("search/{bookId}")] // GET: api/book/search/{bookId}
    public async Task<ActionResult<BookResponseDto>> GetBookById(int bookId)
    {
        var book = await bookService.GetBookByIdAsync(bookId);
        if (book == null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpGet("allbooks")] // GET: api/book/allbooks
    public async Task<ActionResult<List<BookResponseDto>>> GetAllBooks()
    {
        var books = await bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpPut("update/{bookId}")] // PUT: api/book/update/{bookId}
    public async Task<ActionResult<BookResponseDto>> UpdateBook(int bookId, BookDto bookDto)
    {
        var updatedBook = await bookService.UpdateBookAsync(bookId, bookDto);
        if (updatedBook == null)
            return NotFound("Book not found or update failed.");

        return Ok("Book Updated Successfully");
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