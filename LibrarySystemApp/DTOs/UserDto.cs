namespace LibrarySystemApp.DTOs;

public class UserDto
{
    public int Id { get; set; }              // Unique identifier
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required UserRole Role { get; set; }         // e.g., "admin 0" or "student 1"
    public required string Department { get; set; }   // e.g., "IT", "CS", "IS"
    public required int Year { get; set; }
    public required string? Phone { get; set; } // Ensure Phone is included
}