using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories;

namespace LibrarySystemApp.Services;

public class UserService(IUserRepository userRepository, ITokenService tokenService) : IUserService
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetUserByEmailAsync(registerDto.Email.ToLower());
        
        if (existingUser != null) return false; // User already exists
        
        // using var hmac = new HMACSHA3_512();
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

        
        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Role = registerDto.Role,
            Department = registerDto.Department,
            PasswordSalt = salt,
            PasswordHash = hashPassword
        };
        await userRepository.AddUserAsync(user);
        
        return true;
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(loginDto.Email.ToLower());
        if (user == null) return null; // User not found
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid) return null; // Incorrect password

        // Generate and return JWT token
        return tokenService.CreateToken(user);
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
}