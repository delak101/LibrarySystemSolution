using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryContext _context;

        public UserRepository(LibraryContext context)
        {
            _context = context;
        }

        // Retrieve all users
        public async Task<List<User?>> GetAllUsersAsync() => 
        await _context.Users.ToListAsync();

        // Retrieve user by ID
        public async Task<User?> GetUserByIdAsync(int userId) => 
        await _context.Users.FindAsync(userId);

        // Retrieve user by email
        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.Email == (string?)email.ToLower());

        public async Task<List<User?>> GetUsersByNameAsync(string name) =>
            await _context.Users
                .Where(u => u.Name.Contains(name))
                .ToListAsync();

        // Add new user
        public async Task AddUserAsync(User? user)
        {
            user.Email = user.Email.ToLower();
            
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Check if it's a unique constraint violation
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                {
                    throw new InvalidOperationException("A user with this Email or Phone number already exists.");
                }
                throw; // Re-throw other exceptions
            }
        }

        // Update user by ID
        public async Task UpdateUserAsync(User? user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Ensure unique email before updating
            if (await _context.Users.AnyAsync(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && u.Id != user.Id))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // Update user by email
        public async Task UpdateUserByEmailAsync(string email, User user)
        {
            var existingUser = await GetUserByEmailAsync(email);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            existingUser.Name = user.Name;
            existingUser.Department = user.Department;
            existingUser.Phone = user.Phone;
            existingUser.Year = user.Year;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        // Delete user by ID
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }

        // Delete user by email
        public async Task DeleteUserByEmailAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }
    }
}
