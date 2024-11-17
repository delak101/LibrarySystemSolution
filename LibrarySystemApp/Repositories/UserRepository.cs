using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories;

public class UserRepository(LibraryContext context) : IUserRepository
{
    public async Task<User> GetUserByIdAsync(int userId) =>
        await context.Users.FindAsync(userId);
    

    public async Task<User> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public async Task AddUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteUserAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}