namespace TImesheet_TEST.Models;

public class Team : Audit
{
    public int TeamId { get; set; }
    public string? TeamName { get; set; }
    public string? TaskType { get; set; }
    public bool IsManager { get; set; }
    
    public virtual ICollection<Project> Project { get; set; } = new List<Project>();
    
    public virtual ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
}