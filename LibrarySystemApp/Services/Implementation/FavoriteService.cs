using LibrarySystemApp.DTOs;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;

namespace LibrarySystemApp.Services.Implementation;

public class FavoriteService(IFavoriteRepository _favoriteRepository) : IFavoriteService
{
    public async Task<bool> AddFavoriteAsync(int userId, int bookId)
    {
        return await _favoriteRepository.AddFavoriteAsync(userId, bookId);
    }

    public async Task<bool> RemoveFavoriteAsync(int userId, int bookId)
    {
        return await _favoriteRepository.RemoveFavoriteAsync(userId, bookId);
    }

    public async Task<bool> IsFavoriteAsync(int userId, int bookId)
    {
        return await _favoriteRepository.IsFavoriteAsync(userId, bookId);
    }

    public async Task<List<BookDto>> GetUserFavoritesAsync(int userId)
    {
        var books = await _favoriteRepository.GetUserFavoritesAsync(userId);

        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            Shelf = b.Shelf,
            IsAvailable = b.IsAvailable,
            Department = b.Department,
            AssignedYear = b.AssignedYear,
            Image = b.Image,
            PublisherName = b.Publisher?.Name,
            CategoryNames = b.Categories?.Select(c => c.Name).ToList() ?? new List<string>(),
            AuthorNames = b.Authors?.Select(a => a.Name).ToList() ?? new List<string>() // Fix here
        }).ToList();

    }
}