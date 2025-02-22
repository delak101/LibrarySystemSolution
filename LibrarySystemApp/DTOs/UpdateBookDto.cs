namespace LibrarySystemApp.DTOs;

public class UpdateBookDto
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required string Shelf { get; set; }
    public bool isAvailable { get; set; }
    public required string Department { get; set; }
    public int Year { get; set; }
}