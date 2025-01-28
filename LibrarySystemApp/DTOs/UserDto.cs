namespace LibrarySystemApp.DTOs;

public class UserDto
{
    public int Id { get; set; }              // Unique identifier
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required int Role { get; set; }         // e.g., "admin 0" or "student 1"
    public required string Department { get; set; }   // e.g., "IT", "CS", "IS"
    public required int Year { get; set; }  // e.g., 1, 2, 3, 4
    public required string? Phone { get; set; } // Ensure Phone is included
}