namespace LibrarySystemApp.DTOs;

public class UserDto
{
    public int Id { get; set; }              // Unique identifier
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }         // e.g., "admin" or "student"
    public required string Department { get; set; }   // e.g., "IT", "CS", "IS"
    public required string Token { get; set; }
}