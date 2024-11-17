using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces;

public interface IUserRepository
{
    // Retrieves a user by their ID
    Task<User> GetUserByIdAsync(int userId);

    // Retrieves a user by their email (useful for login and checking uniqueness)
    Task<User> GetUserByEmailAsync(string email);

    // Adds a new user to the database
    Task AddUserAsync(User user);

    // Updates an existing user's information
    Task UpdateUserAsync(User user);

    // Deletes a user from the database by their ID
    Task DeleteUserAsync(int userId);

}