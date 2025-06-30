namespace LibrarySystemApp.DTOs;

public class UserDto
{
    public int Id { get; set; } 
    public string? ProfilePicture { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? StudentEmail { get; set; }
    public required long NationalId { get; set; }
    public required string Role { get; set; }
    public required string Phone { get; set; } // Ensure Phone is included
    public required string Department { get; set; }   // e.g., "IT", "CS", "IS"
    public required int Year { get; set; }  // e.g., 1, 2, 3, 4
    public required bool TermsAccepted { get; set; }
}