using LibrarySystemApp.Models;

namespace LibrarySystemApp.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}