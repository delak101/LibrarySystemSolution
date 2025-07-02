using LibrarySystemApp.Models;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Requires authentication
    public class BorrowController(IBorrowService _borrowService) : ControllerBase
    {
        // Request to borrow a book
        [HttpPost("request")]
        public async Task<IActionResult> RequestBorrow(int userId, int bookId)
        {
            try
            {
                var borrow = await _borrowService.RequestBorrowAsync(userId, bookId);
                return Ok(new { message = "Borrow request submitted successfully", borrow });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest((new { error = ex.Message }));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while processing your request" });
            }
            
        }

        // Get all pending borrow requests (Admin)
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingBorrows()
        {
            var pendingRequests = await _borrowService.GetPendingRequestsAsync();
            return Ok(pendingRequests);
        }

        // Get all borrowed books
        [HttpGet("borrowed")]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            var borrowedBooks = await _borrowService.GetBorrowedBooksAsync();
            return Ok(borrowedBooks);
        }

        // Get all overdue books
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueBooks()
        {
            var overdueBooks = await _borrowService.GetOverdueBooksAsync();
            return Ok(overdueBooks);
        }

        // Approve a borrow request (Admin)
        [HttpPost("approve/{borrowId}")]
        public async Task<IActionResult> ApproveBorrow(int borrowId)
        {
            var result = await _borrowService.ApproveBorrowAsync(borrowId);
            if (result)
                return Ok(new { message = "Borrow request approved" }); // Now correctly returns 200 OK
            return BadRequest(new { error = "Failed to approve request" });
        }

        // Deny a borrow request (Admin)
        [HttpPost("deny/{borrowId}")]
        public async Task<IActionResult> DenyBorrow(int borrowId)
        {
            var result = await _borrowService.DenyBorrowAsync(borrowId);
            return result ? Ok(new { message = "Borrow request denied" }) : BadRequest(new { error = "Failed to deny request" });
        }

        // Return a book (Admin)
        [HttpPost("return/{borrowId}")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            var result = await _borrowService.ReturnBookAsync(borrowId);
            if (result)
                return Ok(new { message = "Book returned successfully" }); // Now correctly returns 200 OK
            return BadRequest(new { error = "Failed to return book" });
        }

        // Get borrow history for a user
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetUserBorrowHistory(int userId)
        {
            var history = await _borrowService.GetUserBorrowHistoryAsync(userId);
            return Ok(history);
        }
    }
}
