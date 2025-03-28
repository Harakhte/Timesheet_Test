namespace TImesheet_TEST.Models;

public class Client : Audit
{
    public int ClientId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    
}