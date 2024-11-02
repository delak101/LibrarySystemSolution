using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserContext context, IUserService userService, ITokenService tokenService)
    : ControllerBase
{
    private readonly UserContext _context = context; // Store the UserContext
    private readonly ITokenService _tokenService = tokenService; // Store the token service

    [HttpPost("register")] // POST : user/register
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        var result = await userService.RegisterAsync(registerDto);
        if (!result) 
            return BadRequest("User already exists or registration failed.");
    
        return Ok("User registered successfully.");
    }

    [HttpPost("login")] // POST : user/login
    public async Task<ActionResult<User>> Login(LoginDto loginDto)
    {
        var token = await userService.LoginAsync(loginDto);
        if (token == null) 
            return Unauthorized("Invalid email or password");
    
        return Ok(new { Token = token });
    }
    
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
        var result = await userService.UpdateUserProfileAsync(userId, updateUserDto);
        if (!result) return NotFound("User not found or update failed");
        
        return NoContent();
    }
    
    [HttpDelete("profile/{userId}")] // DELETE deleteUser user/profile/{userId}
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var result = await userService.DeleteUserAsync(userId);
        if (!result) return NotFound("User not found");

        return NoContent();
    }


}