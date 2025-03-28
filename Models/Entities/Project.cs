namespace TImesheet_TEST.Models;

public class Project : Audit
{
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? ProjectNote { get; set; }
    public string? ProjectType { get; set; }
    public string? ProjectCode { get; set; }
    public string? ProjectStatus { get; set; }
    
    public int? ClientId { get; set; }
    public virtual Client Client { get; set; }
    
    public int? TeamId { get; set; }
    public virtual Team Team { get; set; }
    
    public virtual ICollection<TaskProject> TasksProjects { get; set; } = new List<TaskProject>();
}