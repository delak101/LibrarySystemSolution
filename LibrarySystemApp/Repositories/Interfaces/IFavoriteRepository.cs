using LibrarySystemApp.Models;

namespace LibrarySystemApp.Repositories.Interfaces;

public interface IFavoriteRepository
{
    Task<bool> AddFavoriteAsync(int userId, int bookId);
    Task<bool> RemoveFavoriteAsync(int userId, int bookId);
    Task<bool> IsFavoriteAsync(int userId, int bookId);
    Task<List<Book>> GetUserFavoritesAsync(int userId);
}