namespace LibrarySystemApp.DTOs;

public class UpdateUserDto
{
    public string Name { get; set; }         // Optional updated name
    public string Email { get; set; }        // Optional updated email
    public string Department { get; set; }   // Optional updated department
    public string Phone { get; set; }
    public int Year { get; set; }
}