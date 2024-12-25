namespace LibrarySystemApp.DTOs;

public class RegisterDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required UserRole Role { get; set; }  // Admin, Student
    public required string Department { get; set; }  // IT, CS, IS
    public required string Phone { get; set; } // Ensure Phone is required
    public int Year { get; set; }  // Student's academic year, nullable for non-student roles
}