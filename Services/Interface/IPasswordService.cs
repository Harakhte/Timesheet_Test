namespace TImesheet_TEST.Services.Interface;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}