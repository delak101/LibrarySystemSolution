using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoriteController(IFavoriteService _favoriteService) : ControllerBase
{
    [HttpPost("{userId}/{bookId}")]
    public async Task<IActionResult> AddFavorite(int userId, int bookId)
    {
        var result = await _favoriteService.AddFavoriteAsync(userId, bookId);
        if (!result) return BadRequest("Book is already favorited.");
        return Ok("Book added to favorites.");
    }

    [HttpDelete("{userId}/{bookId}")]
    public async Task<IActionResult> RemoveFavorite(int userId, int bookId)
    {
        var result = await _favoriteService.RemoveFavoriteAsync(userId, bookId);
        if (!result) return NotFound("Favorite not found.");
        return Ok("Book removed from favorites.");
    }

    [HttpGet("{userId}/{bookId}")]
    public async Task<IActionResult> IsFavorite(int userId, int bookId)
    {
        var isFavorite = await _favoriteService.IsFavoriteAsync(userId, bookId);
        return Ok(new { IsFavorite = isFavorite });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFavorites(int userId)
    {
        var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
        return Ok(favorites);
    }
}