namespace LibrarySystemApp.DTOs;

public class UpdateUserDto
{
    public string? ProfilePicture { get; set; }
    public string? Name { get; set; }         // Optional updated name
    public string? Email { get; set; }        // Optional updated email
    public string? studentEmail { get; set; }
    public int? NationalId { get; set; }
    public string? Department { get; set; }   // Optional updated department
    public string? Phone { get; set; }
    public int? Year { get; set; }
}