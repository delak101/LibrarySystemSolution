﻿using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpPost("addBook")] // POST: api/book/addBook
    public async Task<ActionResult<BookResponseDto>> AddBook(BookDto bookDto)
    {
        var addedBook = await bookService.AddBookAsync(bookDto);
        if (addedBook == null)
            return BadRequest("Failed to add the book.");

        return Ok(addedBook);
    }
    
    [HttpGet("{bookId}")] // GET: api/book/{bookId}
    public async Task<ActionResult<BookResponseDto>> GetBookById(int bookId)
    {
        var book = await bookService.GetBookByIdAsync(bookId);
        if (book == null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpGet("all")] // GET: api/book/all
    public async Task<ActionResult<List<BookResponseDto>>> GetAllBooks()
    {
        var books = await bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpPut("{bookId}")] // PUT: api/book/{bookId}
    public async Task<ActionResult<BookResponseDto>> UpdateBook(int bookId, BookDto bookDto)
    {
        var updatedBook = await bookService.UpdateBookAsync(bookId, bookDto);
        if (updatedBook == null)
            return NotFound("Book not found or update failed.");

        return Ok(updatedBook);
    }

    [HttpDelete("{bookId}")] // DELETE: api/book/{bookId}
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        var result = await bookService.DeleteBookAsync(bookId);
        if (!result)
            return NotFound("Book not found.");

        return Ok("Book deleted successfully.");
    }

}