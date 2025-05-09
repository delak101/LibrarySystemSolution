using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService _userService)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")] // POST : user/register
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isRegistered = await _userService.RegisterAsync(registerDto);
            if (!isRegistered)
            {
                return BadRequest(new { Message = "User already exists." });
            }
            return Ok(new { Message = "User registered successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, new { Message = "An internal server error occurred." });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")] // POST : user/login
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _userService.LoginAsync(loginDto);
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
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("profile/searchid/{userId}")] // GET findUser user/profile/search/{serId}
    public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
    {
        var user = await _userService.GetUserProfileAsync(userId);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/updateid/{userId}")] // PUT editUser user/profile/update/{userId}
    public async Task<IActionResult> UpdateUserProfile(int userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await _userService.UpdateUserProfileAsync(userId, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok("User updated successfully.");
        // return await GetUserProfile(userId);
    }

    [HttpDelete("profile/deleteid/{userId}")] // DELETE deleteUser user/profile/delete/{userId}
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (!result) return NotFound("User not found");

        return NoContent();
    }

    [HttpGet("profile/search/{email}")] // GET findUser user/profile/search/{email}
    public async Task<ActionResult<UserDto>> GetUserProfile(string email)
    {
        var user = await _userService.GetUserProfileByEmailAsync(email);
        if (user == null) return NotFound("User not found");
        
        return Ok(user);
    }

    [HttpPut("profile/update/{email}")] // PUT editUser user/profile/update/{email}
    public async Task<IActionResult> UpdateUserProfile(string email, UpdateUserDto updateUserDto)
    {
        var updatedUser = await _userService.UpdateUserProfileByEmailAsync(email, updateUserDto);
        if (!updatedUser) return NotFound("User not found or update failed");
        
        return Ok("User updated successfully.");
    }

    [HttpDelete("profile/delete/{email}")] // DELETE deleteUser user/profile/delete/{email}
    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await _userService.DeleteUserByEmailAsync(email);
        if (!result) return NotFound("User not found");

        return NoContent();
    }
    
    [HttpDelete("profile/deleteyear/{year}")]
    public async Task<IActionResult> DeleteUsersByYear(int year)
    {
        bool success = await _userService.DeleteUsersByYearAsync(year);

        if (!success)
            return NotFound($"No users found for year {year}");

        return Ok($"All users from year {year} deleted successfully.");
    }

    [HttpGet("debug-claims")]
    public IActionResult DebugClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(claims);
    }

    [HttpGet("debug-auth")]
    public IActionResult DebugAuth()
    {
        var headers = Request.Headers.Select(h => new { h.Key, h.Value }).ToList();

        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized(new
            {
                message = "Authentication failed! Token might be invalid.",
                headers
            });
        }

        return Ok(new { message = "Authentication successful!", user = User.Identity.Name });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("Access granted: You are an admin!");
    }

    [Authorize]
    [HttpGet("token")]
    public IActionResult GetToken()
    {
        var token = Request.Headers["Authorization"];
        Console.WriteLine($"🔹 Received Token: {token}");
        return Ok(new { Token = token });
    }
    
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
    {
        var result = await _userService.InitiatePasswordReset(request.Email);
        if (!result)
        {
            return BadRequest("Email not found");
        }

        // In a real application, you would send an email here
        // For testing, you can return the token in the response
        return Ok("Password reset initiated. Please check your email.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var result = await _userService.ResetPassword(request.Token, request.NewPassword);
        if (!result)
        {
            return BadRequest("Invalid or expired token");
        }

        return Ok("Password reset successful");
    }

}