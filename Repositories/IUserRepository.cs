using TImesheet_TEST.Models;

namespace TImesheet_TEST.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByUserNameAsync(string userName);
    Task<bool> AddAsync(User user);
    Task<bool> UserExistsAsync(string userName);
    Task<bool> EmailExistsAsync(string email);
}