namespace UserService.DTOs;

public class UpdateUserDto
{
    public string Name { get; set; }         // Optional updated name
    public string Email { get; set; }        // Optional updated email
    public string Role { get; set; }         // Optional updated role
    public string Department { get; set; }   // Optional updated department
}