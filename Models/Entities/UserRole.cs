using System.ComponentModel.DataAnnotations.Schema;

namespace TImesheet_TEST.Models;

public class UserRole
{
    public int UserRoleId { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    [ForeignKey("Role")]
    public int RoleId { get; set; }
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}