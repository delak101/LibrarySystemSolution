using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services.Implementation;

public class UserService(IUserRepository userRepository, ITokenService tokenService) : IUserService
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var email = registerDto.Email.ToLower();
        
        // Check if user already exists
        if (await userRepository.GetUserByEmailAsync(email).ConfigureAwait(false) != null)
        {
            return false; // User already exists
        }
        
        // Hash password using the salt
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        
        var user = new User
        {
            Name = registerDto.Name,
            Email = email,
            PasswordHash = hashPassword,
            Role = registerDto.Role,
            Department = registerDto.Department,
            Phone = registerDto.Phone,
            Year = registerDto.Year
        };
        try
        {
            await userRepository.AddUserAsync(user);
            return true;
        }
        catch (DbUpdateException ex)
        {
            // Log error details and return failure
            Console.WriteLine($"Error saving user: {ex.Message}");
            throw new InvalidOperationException("Failed to save user.");
        }
    }

    public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(loginDto.Email.ToLower());
        if (user == null) 
            throw new UnauthorizedAccessException("User does not exist.");

        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid) 
            throw new UnauthorizedAccessException("Invalid password.");
            
        // Generate and return JWT token
        var token = tokenService.CreateToken(user);
        
        return new UserResponseDto
        {
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                Token = token
            },
            Token = token
        };

    }
    
    public async Task<UserDto> GetUserProfileAsync(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Department = user.Department,
            Token = null
        };
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        // Update only the modified fields
        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email ?? user.Email;
        user.Department = updateUserDto.Department ?? user.Department;
        await userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        await userRepository.DeleteUserAsync(userId);
        return true;
    }
    
    public bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
    {
        var hashInput = BCrypt.Net.BCrypt.HashPassword(inputPassword, storedSalt);
        return hashInput == storedHash;
    }

}