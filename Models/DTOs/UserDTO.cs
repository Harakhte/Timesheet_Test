namespace TImesheet_TEST.Models.DTOs;

public class UserDTO
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}