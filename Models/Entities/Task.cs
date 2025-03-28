namespace TImesheet_TEST.Models;

public class Task : Audit
{
    public int TaskId { get; set; }
    public string? TaskName { get; set; }
    public string? TaskType { get; set; }
    public bool Billable { get; set; }
    
    public virtual ICollection<TaskProject> TasksProjects { get; set; } = new List<TaskProject>();
}