using System.ComponentModel.DataAnnotations;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.DTOs;

public class BookDto
{
    [Required]
    [MaxLength(500)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Shelf { get; set; }

    public bool IsAvailable { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }
    public int? AssignedYear { get; set; }
    public string? Image { get; set; }
    
    public string? PublisherName { get; set; }

    public List<string> CategoryNames { get; set; } = new List<string>();

    public List<string> AuthorNames { get; set; } = new List<string>();
}