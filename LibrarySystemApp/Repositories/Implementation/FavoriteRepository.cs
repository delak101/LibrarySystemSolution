using LibrarySystemApp.Data;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories.Implementation;

public class FavoriteRepository(LibraryContext _context) : IFavoriteRepository
{
    public async Task<bool> AddFavoriteAsync(int userId, int bookId)
    {
        if (await _context.Favorites.AnyAsync(f => f.UserId == userId && f.BookId == bookId))
            return false; // Already favorited

        var favorite = new Favorite { UserId = userId, BookId = bookId };
        _context.Favorites.Add(favorite);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFavoriteAsync(int userId, int bookId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.BookId == bookId);

        if (favorite == null)
        {
            Console.WriteLine($"❌ Favorite not found: UserId={userId}, BookId={bookId}");
            return false; // Book is not in favorites
        }

        _context.Favorites.Remove(favorite);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsFavoriteAsync(int userId, int bookId)
    {
        return await _context.Favorites.AnyAsync(f => f.UserId == userId && f.BookId == bookId);
    }

    public async Task<List<Book>> GetUserFavoritesAsync(int userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.Book)
            .ToListAsync();
    }
}