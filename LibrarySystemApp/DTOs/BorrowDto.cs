using LibrarySystemApp.Models;

namespace LibrarySystemApp.DTOs;

public class BorrowDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }  // Get user's name
    public int BookId { get; set; }
    public string BookTitle { get; set; } // Get book's title
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public string? BookImg { get; internal set; }
    public string StudentPfp { get; internal set; }
    public ICollection<Author> BookAuthor { get; internal set; }
    public string BookShelf { get; internal set; }
}