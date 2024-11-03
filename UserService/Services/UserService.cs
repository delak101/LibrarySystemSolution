using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;
using BCrypt.Net;
namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;

    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email.ToLower());
        
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
            // PasswordSalt = hmac.Key //I'll use this key in the login to hash the entered password and compare "hashedPasswords"
            // PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = salt,
            PasswordHash = hashPassword
        };
        await _userRepository.AddUserAsync(user);
        
        return true;
    }

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email.ToLower());
        if (user == null) return null; // User not found

        // using var hmac = new HMACSHA512(user.PasswordSalt);
        // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // Validate hashed password
        // for (int i = 0; i < computedHash.Length; i++)
        //     if (computedHash[i] != user.PasswordHash[i])
        //         return null; // Incorrect password

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid) return null; // Incorrect password

        // Generate and return JWT token
        return _tokenService.CreateToken(user);
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
            Department = user.Department,
            Token = null
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