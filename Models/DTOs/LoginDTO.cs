using System.ComponentModel.DataAnnotations;

namespace TImesheet_TEST.Models.DTOs;

public class LoginDTO
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}