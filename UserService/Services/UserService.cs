using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;
using UserService.Utilities;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;  // For password hashing

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email.ToLower());
        // if (existingUser != null) throw new Exception($"{registerDto.Email} already Exists"); // User already exists
        if (existingUser != null) return false; // User already exists
        
        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Role = registerDto.Role,
            Department = registerDto.Department
        };
        user.PasswordHash = _passwordHasher.HashPassword(registerDto.Password);

        await _userRepository.AddUserAsync(user);
        return true;

    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null) return null;  // User not found

        // Verify password
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.PasswordHash, loginDto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed) return null;

        // Generate JWT token (you would replace this with a real JWT generation)
        return "GeneratedJWTToken";
    }

    public async Task<UserDto> GetUserProfileAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Department = user.Department
        };
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        // Update only the modified fields
        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email ?? user.Email;
        user.Role = updateUserDto.Role ?? user.Role;
        user.Department = updateUserDto.Department ?? user.Department;

        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        await _userRepository.DeleteUserAsync(userId);
        return true;
    }
}