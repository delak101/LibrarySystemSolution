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
            var registeredUser = await userService.RegisterAsync(registerDto);
            if (!registeredUser)
            {
                return BadRequest(new { Message = "User already exists or registration failed." });
            }
            return Ok(new { Message = "User registered successfully." });
        }
        catch (Exception ex)
        {
            // Log exception
            return StatusCode(500, new { Message = "An internal server error occurred.", Details = ex.Message });
        }
    
        //"User registered successfully."
        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")] // POST : user/login
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await userService.LoginAsync(loginDto);
        if (token == null) 
            return Unauthorized("Invalid email or password");
    
        return Ok(new { Token = token });
    }
    
    // [Authorize(policy:"AdminOnly")]
    [HttpGet("profile/{userId}")] // GET findUser user/profile/{userId}
    public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
    {
        var user = await userService.GetUserProfileAsync(userId);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/{userId}")] // PUT editUser user/profile/{userId}
    public async Task<IActionResult> UpdateUserProfile(int userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await userService.UpdateUserProfileAsync(userId, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok(updatedUser);
    }
    // [Authorize(policy:"AdminOnly")]
    [HttpDelete("profile/{userId}")] // DELETE deleteUser user/profile/{userId}
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        if (!result) return NotFound("User not found");

        return NoContent();
    }
}