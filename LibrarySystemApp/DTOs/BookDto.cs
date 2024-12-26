namespace LibrarySystemApp.DTOs;

public class BookDto
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required string Shelf { get; set; }
    public bool State { get; set; } // Available or Borrowed
    public required string Department { get; set; } // IT, CS, IS
    public required int Year { get; set; } // First-year, second-year, etc.
}