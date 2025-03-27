using System.ComponentModel.DataAnnotations;

namespace LibrarySystemApp.DTOs;

public class LoginDto
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}