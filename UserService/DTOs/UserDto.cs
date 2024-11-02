namespace UserService.DTOs;

public class UserDto
{
    public int Id { get; set; }              // Unique identifier
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }         // e.g., "admin" or "student"
    public string Department { get; set; }   // e.g., "IT", "CS", "IS"
}