namespace LibrarySystemApp.DTOs;

public class BookResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Shelf { get; set; }
    public bool State { get; set; }
    public string Department { get; set; }
    public int Year { get; set; }
}