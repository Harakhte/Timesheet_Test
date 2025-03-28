using System.ComponentModel.DataAnnotations;

namespace TImesheet_TEST.Models.DTOs;

public class AuthDTO
{
    public class AuthResponseViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserResponseViewModel User { get; set; }
    }

    public class UserResponseViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}