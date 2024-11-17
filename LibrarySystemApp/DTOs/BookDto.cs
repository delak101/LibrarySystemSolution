namespace LibrarySystemApp.DTOs;

public class BookDto
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Shelf { get; set; }
    public bool State { get; set; } // Available or Borrowed
    public string Department { get; set; } // IT, CS, IS
    public int Year { get; set; } // First-year, second-year, etc.
}