using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TImesheet_TEST.Context;
using TImesheet_TEST.Models;
using Microsoft.EntityFrameworkCore;
using TImesheet_TEST.Models.DTOs;
using TImesheet_TEST.Services;

namespace TImesheet_TEST.Services
{
    public class AuthService
    {
        private readonly Timesheet_DbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(Timesheet_DbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<UserDTO> Register(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == registerDto.UserName))
                throw new Exception("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("Email already exists");

            var passwordHash = HashPassword(registerDto.Password);

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assign default "User" role
            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = 2 // Default "User" role
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            var roles = new List<string> { "User" };

            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            };
        }

        public async Task<string> Login(LoginDTO loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

            if (user == null)
                throw new Exception("User not found");

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new Exception("Wrong password");

            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.RoleId,
                    (ur, r) => r.RoleName)
                .ToListAsync();

            var token = _jwtService.GenerateToken(user, roles);

            return token;
        }

        private string HashPassword(string password)
        {
            // Using SHA256 hashing (consider using PBKDF2 for better security)
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public void UpdatePassword(User user, string newPassword)
        {
            var passwordHash = HashPassword(newPassword);
            user.PasswordHash = passwordHash;
        }
        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}