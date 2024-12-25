using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, ITokenService tokenService)
    : ControllerBase
{
    [HttpPost("register")] // POST : user/register
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        try
        {
            var isRegistered = await userService.RegisterAsync(registerDto);
            if (!isRegistered)
            {
                return BadRequest(new { Message = "User already exists or registration failed." });
            }
            return Ok(new { Message = "User registered successfully." });
        }
        catch (InvalidOperationException ex)
        {
            // Log exception (e.g., logger.LogError(ex, "Invalid operation during registration"))
            return BadRequest(new { hint = "problem here", Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log exception (e.g., logger.LogError(ex, "Internal server error"))
            return StatusCode(500, new { Message = "An internal server error occurred.", Details = ex.Message });
        }
    }

    [HttpPost("login")] // POST : user/login
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var response = await userService.LoginAsync(loginDto);
            return Ok(response);

        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error.", Details = ex.Message });
        }
    }

    [HttpGet("all")] // GET: api/user/all
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    // [Authorize(policy:"AdminOnly")]
    [HttpGet("profile/searchid/{userId}")] // GET findUser user/profile/search/{serId}
    public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
    {
        var user = await userService.GetUserProfileAsync(userId);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/updateid/{userId}")] // PUT editUser user/profile/update/{userId}
    public async Task<IActionResult> UpdateUserProfile(int userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await userService.UpdateUserProfileAsync(userId, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok("User updated successfully.");
        // return await GetUserProfile(userId);
    }
    // [Authorize(policy:"AdminOnly")]
    [HttpDelete("profile/deleteid/{userId}")] // DELETE deleteUser user/profile/delete/{userId}
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        if (!result) return NotFound("User not found");

        return NoContent();
    }

    [HttpGet("profile/search/{email}")] // GET findUser user/profile/search/{email}
    public async Task<ActionResult<UserDto>> GetUserProfile(string email)
    {
        var user = await userService.GetUserProfileByEmailAsync(email);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/update/{email}")] // PUT editUser user/profile/update/{email}
    public async Task<IActionResult> UpdateUserProfile(string email, UpdateUserDto updateUserDto)
    {
        var updatedUser = await userService.UpdateUserProfileByEmailAsync(email, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok("User updated successfully.");
    }

    [HttpDelete("profile/delete/{email}")] // DELETE deleteUser user/profile/delete/{email}
    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await userService.DeleteUserByEmailAsync(email);
        if (!result) return NotFound("User not found");

        return NoContent();
    }
}