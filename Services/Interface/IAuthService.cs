using TImesheet_TEST.Models.DTOs;

namespace TImesheet_TEST.Services.Interface;

public interface IAuthService
{
    Task<AuthDTO.AuthResponseViewModel> RegisterAsync(RegisterDTO registerViewModel);
    Task<AuthDTO.AuthResponseViewModel> LoginAsync(LoginDTO loginViewModel);
}