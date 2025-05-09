namespace LibrarySystemApp.DTOs;

public class RegisterDto
{
    public string pfp { get; set; }
    public required string name { get; set; }
    public required string email { get; set; }
    public string? studentEmail { get; set; }
    public required string password { get; set; }
    public int nationalId { get; set; }
    // public required UserRole Role { get; set; }  // Admin, Student
    public required string department { get; set; }  // IT, CS, IS
    public required string phone { get; set; }
    public required bool termsAccepted { get; set; }
    public int year { get; set; }  // Student's academic year, nullable for non-student roles
}