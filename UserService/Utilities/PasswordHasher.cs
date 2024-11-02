using Microsoft.AspNetCore.Identity;

namespace UserService.Utilities;

public class PasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher; // Using a non-specific type as we don't need user details here.

    public PasswordHasher()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    public string HashPassword(string password)
    {
        // We pass null for the user parameter since we're using a non-specific type
        return _passwordHasher.HashPassword(null, password);
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        // Again, we pass null for the user parameter
        return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
    }

}