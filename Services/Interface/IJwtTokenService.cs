using TImesheet_TEST.Models;

namespace TImesheet_TEST.Services.Interface;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}