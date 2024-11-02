using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class UserController : ControllerBase
{
    
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        var result = await _userService.RegisterAsync(registerDto);
        if (!result) 
            return BadRequest("User already exists or registration failed.");
    
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(LoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto);
        return token != null ? Ok(new { Token = token }) : Unauthorized("Invalid credentials");
    }
}