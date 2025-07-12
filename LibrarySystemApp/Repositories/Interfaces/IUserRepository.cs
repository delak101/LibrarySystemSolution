using LibrarySystemApp.Models;

namespace LibrarySystemApp.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User?>> GetAllUsersAsync();
    
    // Retrieves all approved users (students)
    Task<List<User?>> GetAllApprovedUsersAsync();
    
    // Retrieves a user by their ID
    Task<User?> GetUserByIdAsync(int userId);
    
    // Retrieves a user by their email (useful for login and checking uniqueness)
    Task<User?> GetUserByEmailAsync(string email);

    // Retrieves a user by either email or student email (useful for flexible login)
    Task<User?> GetUserByEmailOrStudentEmailAsync(string email);

    // Check if user exists by either email or student email (useful for registration validation)
    Task<bool> UserExistsByEmailOrStudentEmailAsync(string email);

    Task<List<User?>> GetUserByNameAsync(string name);
    
    // Adds a new user to the database
    Task AddUserAsync(User? user);

    // Updates an existing user's information
    Task UpdateUserAsync(User? user);

    // Deletes a user from the database by their ID
    Task DeleteUserAsync(int userId);

    // Update user by email
    Task UpdateUserByEmailAsync(string email, User user);
    // Delete user by email
    Task DeleteUserByEmailAsync(string email);
    Task<int> DeleteUsersByYearAsync(int year);

}