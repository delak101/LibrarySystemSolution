using LibrarySystemApp.DTOs;

namespace LibrarySystemApp.Services.Interfaces;

public interface IFavoriteService
{
    Task<bool> AddFavoriteAsync(int userId, int bookId);
    Task<bool> RemoveFavoriteAsync(int userId, int bookId);
    Task<bool> IsFavoriteAsync(int userId, int bookId);
    Task<List<BookDto>> GetUserFavoritesAsync(int userId);
}