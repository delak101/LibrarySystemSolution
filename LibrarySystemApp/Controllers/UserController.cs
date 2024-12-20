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
    
    // [Authorize(policy:"AdminOnly")]
    [HttpGet("profile/search/{userId}")] // GET findUser user/profile/search/{serId}
    public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
    {
        var user = await userService.GetUserProfileAsync(userId);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/update/{userId}")] // PUT editUser user/profile/update/{userId}
    public async Task<IActionResult> UpdateUserProfile(int userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await userService.UpdateUserProfileAsync(userId, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok("User updated successfully.");
        // return await GetUserProfile(userId);
    }
    // [Authorize(policy:"AdminOnly")]
    [HttpDelete("profile/delete/{userId}")] // DELETE deleteUser user/profile/delete/{userId}
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        if (!result) return NotFound("User not found");

        return NoContent();
    }
}