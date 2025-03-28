using Microsoft.EntityFrameworkCore;
using TImesheet_TEST.Context;
using TImesheet_TEST.Models;

namespace TImesheet_TEST.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Timesheet_DbContext _context;

    public UserRepository(Timesheet_DbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        return await _context.Users.AnyAsync(u => u.UserName == userName);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}