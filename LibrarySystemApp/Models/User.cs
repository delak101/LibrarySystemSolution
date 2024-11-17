using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Required]
    [MaxLength(256)] // Ensures compatibility with SQL Server
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string Role { get; set; }  // Admin, Student
    public string Department { get; set; }  // IT, CS, IS
    public int Phone { get; set; }
    public int year { get; set; }
}