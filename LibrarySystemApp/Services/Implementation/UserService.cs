using LibrarySystemApp.Data;
using LibrarySystemApp.DTOs;
using LibrarySystemApp.Interfaces;
using LibrarySystemApp.Models;
using LibrarySystemApp.Repositories.Interfaces;
using LibrarySystemApp.Services.Interfaces;
using LibrarySystemApp.Services;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApp.Services.Implementation;

public class UserService(
    IUserRepository _userRepository,
    ITokenService _tokenService,
    LibraryContext _context,
    IEmailService _emailService,
    IConfiguration _configuration,
    INotificationService _notificationService
) : IUserService
{
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var email = registerDto.email.ToLower();
        var studentEmail = registerDto.studentEmail?.ToLower();

        // Check if a user already exists with the same regular email
        if (await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false) != null)
        {
            return false; // User already exists with this email
        }

        // Check if a user already exists with the same student email (if provided)
        if (!string.IsNullOrEmpty(studentEmail))
        {
            var existingUserWithStudentEmail = await _context.Users
                .AnyAsync(u => u.StudentEmail != null && u.StudentEmail.ToLower() == studentEmail);
            if (existingUserWithStudentEmail)
            {
                return false; // User already exists with this student email
            }
        }

        // Hash password using the salt
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.password);


        var user = new User
        {
            ProfilePicture = registerDto.pfp,
            Name = registerDto.name,
            Email = email,
            StudentEmail = registerDto.studentEmail,
            PasswordHash = hashPassword,
            NationalId = registerDto.nationalId,
            Department = registerDto.department,
            Phone = registerDto.phone,
            Year = registerDto.year,
            TermsAccepted = registerDto.termsAccepted,
            PasswordResetToken = null,
            PasswordResetTokenExpiry = null
        };
        try
        {
            await _userRepository.AddUserAsync(user);
            return true;
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException?.Message ?? ex.Message;
            throw new InvalidOperationException($"Failed to save user: {innerException}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Try to find user by either regular email or student email
        var user = await _userRepository.GetUserByEmailOrStudentEmailAsync(loginDto.Email.ToLower());
        if (user == null)
            throw new UnauthorizedAccessException("User does not exist.");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid password.");

        // Check if user is approved (except for Admin users)
        if (user.Role != "Admin" && !user.IsApproved)
            throw new UnauthorizedAccessException("Account is pending approval. Please wait for admin approval.");

        var token = _tokenService.CreateToken(user);

        return new UserResponseDto
        {
            User = MapToResponseDto(user),
            Token = token
        };
    }

    public async Task<UserDto?> GetUserProfileByNameAsync(string name)
    {
        var users = await _userRepository.GetUserByNameAsync(name); // Fetch users by name using repository
        var firstUser = users.FirstOrDefault(); // Get the first matching user (assuming there could be many)
        return firstUser == null ? null : MapToResponseDto(firstUser); // Map to DTO if user exists
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllApprovedUsersAsync();
        return users.Where(u => u != null).Select(MapToResponseDto).ToList();
    }

    public async Task<UserDto> GetUserProfileAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user == null ? null : MapToResponseDto(user);
    }

    public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateUserDto)
    {
        // Retrieve the user from the repository
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            // User not found, throw an exception or return false
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        // Update only the fields provided in the DTO; leave others unchanged
        user.ProfilePicture = updateUserDto.ProfilePicture ?? user.ProfilePicture;
        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email?.ToLower() ?? user.Email; // Ensure email consistency
        user.StudentEmail = updateUserDto.studentEmail ?? user.StudentEmail;
        user.Department = updateUserDto.Department ?? user.Department;
        user.NationalId = updateUserDto.NationalId ?? user.NationalId;
        user.Year = updateUserDto.Year ?? user.Year;
        user.Phone = updateUserDto.Phone ?? user.Phone;

        try
        {
            // Save the updated user to the database
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
        catch (DbUpdateException ex)
        {
            // Handle cases like unique constraints (e.g., duplicate email)
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            throw new InvalidOperationException($"Failed to update user: {errorMessage}");
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors
            throw new InvalidOperationException($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        await _userRepository.DeleteUserAsync(userId);
        return true;
    }

    public async Task<UserDto?> GetUserProfileByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        return user == null ? null : MapToResponseDto(user);
    }

    public async Task<bool> UpdateUserProfileByEmailAsync(string email, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) return false;

        // Update only the fields provided in the DTO; leave others unchanged
        user.ProfilePicture = updateUserDto.ProfilePicture ?? user.ProfilePicture;
        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email?.ToLower() ?? user.Email; // Ensure email consistency
        user.StudentEmail = updateUserDto.studentEmail ?? user.StudentEmail;
        user.Department = updateUserDto.Department ?? user.Department;
        user.NationalId = updateUserDto.NationalId ?? user.NationalId;
        user.Year = updateUserDto.Year ?? user.Year;
        user.Phone = updateUserDto.Phone ?? user.Phone;
        await _userRepository.UpdateUserByEmailAsync(email, user);
        return true;
    }

    public async Task<bool> DeleteUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) return false;

        await _userRepository.DeleteUserByEmailAsync(email);
        return true;
    }

    public async Task<bool> DeleteUsersByYearAsync(int year)
    {
        int deletedCount = await _userRepository.DeleteUsersByYearAsync(year);
        return deletedCount > 0;
    }


    public async Task<bool> InitiatePasswordReset(string email)
    {        
        // Try to find user by either regular email or student email
        var user = await _userRepository.GetUserByEmailOrStudentEmailAsync(email);
        if (user == null) return false;

        // Generate a 6-digit numeric token
        var random = new Random();
        var token = random.Next(100000, 999999).ToString();
        user.PasswordResetToken = token;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Reduced to 1 hour for security

        await _context.SaveChangesAsync();

        // Get the base URL from configuration
        var baseUrl = _configuration["appsettings:BaseUrl"] ?? "https://fci-library.live";        // Create the reset link
        var resetLink = $"{baseUrl}/api/user/reset-password?token={user.PasswordResetToken}";

        // Create email content
        var subject = "Your Password Reset Code - FCI Library System";
        var content = $@"
            Hello,

            You have requested to reset your password for your FCI Library System account.

            Your password reset code is:

            {user.PasswordResetToken}

            Please enter this code on the password reset page: {baseUrl}/api/user/reset-password

            This code will expire in 1 hour for security purposes.

            If you didn't request this password reset, please ignore this email and ensure your account is secure.

            Best regards,
            FCI Library System Team
        ";

        // Send the email
        return await _emailService.SendEmailAsync(user.Email, subject, content);
    }

    public async Task<bool> ResetPassword(string token, string newPassword)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token &&
                                      u.PasswordResetTokenExpiry > DateTime.UtcNow);

        if (user == null) return false;

        // Hash the new password (assuming you're using BCrypt)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetPendingUsersAsync()
    {
        var pendingUsers = await _context.Users
            .Where(u => !u.IsApproved && u.Role != "Admin")
            .Select(u => new UserDto
            {
                Id = u.Id,
                ProfilePicture = u.ProfilePicture,
                Name = u.Name,
                Email = u.Email,
                StudentEmail = u.StudentEmail,
                NationalId = u.NationalId,
                Role = u.Role,
                Phone = u.Phone,
                Department = u.Department,
                Year = u.Year,
                TermsAccepted = u.TermsAccepted,
                IsApproved = u.IsApproved,
                ApprovedAt = u.ApprovedAt
            })
            .ToListAsync();

        return pendingUsers;
    }

    public async Task<bool> ApproveUserAsync(int userId, int approvedBy)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.IsApproved = true;
        user.ApprovedAt = DateTime.UtcNow;
        user.ApprovedBy = approvedBy;

        await _context.SaveChangesAsync();

        // Send approval notification
        try
        {
            await _notificationService.SendUserApprovalNotificationAsync(userId, user.Name);
        }
        catch (Exception ex)
        {
            // Log the error but don't fail the approval process
            // You might want to inject ILogger here for proper logging
            Console.WriteLine($"Failed to send approval notification: {ex.Message}");
        }

        return true;
    }

    public async Task<bool> RejectUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        // Send rejection notification before deleting
        try
        {
            await _notificationService.SendUserRejectionNotificationAsync(userId, user.Name);
        }
        catch (Exception ex)
        {
            // Log the error but don't fail the rejection process
            Console.WriteLine($"Failed to send rejection notification: {ex.Message}");
        }

        // Instead of deleting, we could mark as rejected
        // For now, we'll delete the user
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> GetUserApprovalStatusAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.IsApproved ?? false;
    }

    private static UserDto MapToResponseDto(User? user)
    {
        if (user == null) return null!;
        return new UserDto
        {
            Id = user.Id,
            ProfilePicture = user.ProfilePicture,
            Name = user.Name,
            Email = user.Email,
            StudentEmail = user.StudentEmail,
            NationalId = user.NationalId,
            Role = user.Role,
            Phone = user.Phone,
            Department = user.Department,
            Year = user.Year,
            TermsAccepted = user.TermsAccepted,
            IsApproved = user.IsApproved,
            ApprovedAt = user.ApprovedAt
        };
    }
}