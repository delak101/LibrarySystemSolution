namespace LibrarySystemApp.DTOs;

public class UserResponseDto
{
    public required UserDto User { get; set; }
    public required string Token { get; set; }
}