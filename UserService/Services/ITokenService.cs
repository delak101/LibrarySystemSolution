using UserService.Models;

namespace UserService.Services;

public interface ITokenService
{
    string CreateToken(User user);
}