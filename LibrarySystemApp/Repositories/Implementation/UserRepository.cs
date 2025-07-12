using LibrarySystemApp.Data;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Repositories.Implementation
{
    public class UserRepository(LibraryContext _context) : IUserRepository
    {
        // Retrieve all users
        public async Task<List<User?>> GetAllUsersAsync() => 
        await _context.Users.ToListAsync();

        // Retrieve all approved users (students)
        public async Task<List<User?>> GetAllApprovedUsersAsync() => 
            await _context.Users.Where(u => u.IsApproved).ToListAsync();

        // Retrieve user by ID
        public async Task<User?> GetUserByIdAsync(int userId) => 
        await _context.Users.FindAsync(userId);

        // Retrieve user by email
        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    
        // Retrieve user by email or student email
        public async Task<User?> GetUserByEmailOrStudentEmailAsync(string email) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() || 
                                         (u.StudentEmail != null && u.StudentEmail.ToLower() == email.ToLower()));

        // Check if user exists by either email or student email
        public async Task<bool> UserExistsByEmailOrStudentEmailAsync(string email) =>
            await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() || 
                              (u.StudentEmail != null && u.StudentEmail.ToLower() == email.ToLower()));

        public async Task<List<User?>> GetUserByNameAsync(string name) =>
            await _context.Users
                .Where(u => u.Name.ToLower().Contains(name.ToLower()))
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
                    throw new InvalidOperationException("A user with this email or phone number already exists.");
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
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower() && u.Id != user.Id))
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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                // Delete related records first to avoid foreign key constraint violations
                
                // Delete user's favorites
                var favorites = await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();
                _context.Favorites.RemoveRange(favorites);
                
                // Delete user's reviews
                var reviews = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
                _context.Reviews.RemoveRange(reviews);
                
                // Delete user's borrow records
                var borrows = await _context.Borrows.Where(b => b.UserId == userId).ToListAsync();
                _context.Borrows.RemoveRange(borrows);
                
                // Delete user's device tokens (should cascade automatically, but let's be explicit)
                var deviceTokens = await _context.UserDeviceTokens.Where(dt => dt.UserId == userId).ToListAsync();
                _context.UserDeviceTokens.RemoveRange(deviceTokens);
                
                // Finally, delete the user
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Delete user by email
        public async Task DeleteUserByEmailAsync(string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                // Delete related records first to avoid foreign key constraint violations
                
                // Delete user's favorites
                var favorites = await _context.Favorites.Where(f => f.UserId == user.Id).ToListAsync();
                _context.Favorites.RemoveRange(favorites);
                
                // Delete user's reviews
                var reviews = await _context.Reviews.Where(r => r.UserId == user.Id).ToListAsync();
                _context.Reviews.RemoveRange(reviews);
                
                // Delete user's borrow records
                var borrows = await _context.Borrows.Where(b => b.UserId == user.Id).ToListAsync();
                _context.Borrows.RemoveRange(borrows);
                
                // Delete user's device tokens (should cascade automatically, but let's be explicit)
                var deviceTokens = await _context.UserDeviceTokens.Where(dt => dt.UserId == user.Id).ToListAsync();
                _context.UserDeviceTokens.RemoveRange(deviceTokens);
                
                // Finally, delete the user
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<int> DeleteUsersByYearAsync(int year)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var usersToDelete = await _context.Users.Where(u => u.Year == year).ToListAsync();
                if (!usersToDelete.Any()) return 0;

                var userIds = usersToDelete.Select(u => u.Id).ToList();

                // Delete related records first to avoid foreign key constraint violations
                
                // Delete favorites for all users
                var favorites = await _context.Favorites.Where(f => userIds.Contains(f.UserId)).ToListAsync();
                _context.Favorites.RemoveRange(favorites);
                
                // Delete reviews for all users
                var reviews = await _context.Reviews.Where(r => userIds.Contains(r.UserId)).ToListAsync();
                _context.Reviews.RemoveRange(reviews);
                
                // Delete borrow records for all users
                var borrows = await _context.Borrows.Where(b => userIds.Contains(b.UserId)).ToListAsync();
                _context.Borrows.RemoveRange(borrows);
                
                // Delete device tokens for all users
                var deviceTokens = await _context.UserDeviceTokens.Where(dt => userIds.Contains(dt.UserId)).ToListAsync();
                _context.UserDeviceTokens.RemoveRange(deviceTokens);
                
                // Finally, delete the users
                _context.Users.RemoveRange(usersToDelete);
                var result = await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
