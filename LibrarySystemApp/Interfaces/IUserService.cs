using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces;

public interface IUserService
{
    // Registers a new user and returns a success flag
    Task<bool> RegisterAsync(RegisterDto registerDto);

    // Authenticates a user and returns a token if successful
    Task<string?> LoginAsync(LoginDto loginDto);

    // Retrieves user details for profile display
    Task<UserDto> GetUserProfileAsync(int userId);

    // Updates user profile details
    Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto);

    // Deletes a user account
    Task<bool> DeleteUserAsync(int userId);

}