namespace LibrarySystemApp.DTOs;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }  // Admin, Student
    public string Department { get; set; }  // IT, CS, IS
    public string Phone { get; set; }
    public int Year { get; set; }  // Student's academic year
}