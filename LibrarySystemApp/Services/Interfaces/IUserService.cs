﻿using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces;

public interface IUserService
{
    // Registers a new user and returns a success flag
    Task<bool> RegisterAsync(RegisterDto registerDto);

    // Authenticates a user and returns a token if successful
    Task<UserResponseDto> LoginAsync(LoginDto loginDto);

    // Retrieves user details for profile display
    Task<UserDto> GetUserProfileAsync(int userId);
    
    // Retrive a user by name
    Task<UserDto?> GetUserProfileByNameAsync(string name);

    // Retrieves all users in database
    Task<List<UserDto>> GetAllUsersAsync();

    // Updates user profile details
    Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto);

    // Deletes a user account
    Task<bool> DeleteUserAsync(int userId);
    
    // Retrieve user by email
    Task<UserDto?> GetUserProfileByEmailAsync(string email);

    // Update user by email
    Task<bool> UpdateUserProfileByEmailAsync(string email, UpdateUserDto updateUserDto);
    
    // Delete user by email
    Task<bool> DeleteUserByEmailAsync(string email);
    
    // Bulk delete users by year
    Task<bool> DeleteUsersByYearAsync(int year);

    Task<bool> InitiatePasswordReset(string email);

    Task<bool> ResetPassword(string token, string newPassword);

    // User approval methods
    Task<List<UserDto>> GetPendingUsersAsync();
    Task<bool> ApproveUserAsync(int userId, int approvedBy);
    Task<bool> RejectUserAsync(int userId);
    Task<bool> GetUserApprovalStatusAsync(int userId);


}