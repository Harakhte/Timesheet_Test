using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TImesheet_TEST.Context;
using TImesheet_TEST.Models;
using TImesheet_TEST.Models.DTOs;
using TImesheet_TEST.Repositories;
using TImesheet_TEST.Services.Interface;

namespace TImesheet_TEST.Services;

public class AuthService : IAuthService
{
     private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    private readonly Timesheet_DbContext _context;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordService passwordService,
        IMapper mapper,
        Timesheet_DbContext context)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordService = passwordService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<AuthDTO.AuthResponseViewModel> LoginAsync(LoginDTO loginViewModel)
    {
        var user = await _userRepository.GetByUserNameAsync(loginViewModel.UserName);
        if (user == null || !_passwordService.VerifyPassword(loginViewModel.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password");

        var token = _jwtTokenService.GenerateToken(user);
        var userResponse = _mapper.Map<AuthDTO.UserResponseViewModel>(user);
        
        return new AuthDTO.AuthResponseViewModel
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userResponse
        };
    }

    public async Task<AuthDTO.AuthResponseViewModel> RegisterAsync(RegisterDTO registerViewModel)
    {
        if (await _userRepository.UserExistsAsync(registerViewModel.UserName))
            throw new ArgumentException("Username already exists");

        if (await _userRepository.EmailExistsAsync(registerViewModel.Email))
            throw new ArgumentException("Email already in use");

        var user = new User
        {
            UserName = registerViewModel.UserName,
            Email = registerViewModel.Email,
            PasswordHash = _passwordService.HashPassword(registerViewModel.Password)
        };

        // Assign default "User" role
        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 1);
        if (defaultRole != null)
        {
            user.UserRoles.Add(new UserRole { RoleId = defaultRole.RoleId });
        }

        if (!await _userRepository.AddAsync(user))
            throw new Exception("User creation failed");

        var token = _jwtTokenService.GenerateToken(user);
        var userResponse = _mapper.Map<AuthDTO.UserResponseViewModel>(user);
        
        return new AuthDTO.AuthResponseViewModel
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userResponse
        };
    }
}