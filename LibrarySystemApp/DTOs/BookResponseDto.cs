using LibrarySystemApp.Models;

namespace LibrarySystemApp.DTOs;

public class BookResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Shelf { get; set; }
    public bool IsAvailable { get; set; }
    public string Department { get; set; }
    public int? AssignedYear { get; set; }
    public string Image { get; set; }
        
    public List<string> CategoryNames { get; set; }  // List of category names instead of category objects
    public List<string> AuthorNames { get; set; }  // List of author names instead of author objects

}