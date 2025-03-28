namespace TImesheet_TEST.Models;

public class TaskProject
{
    public int TaskProjectId { get; set; }
    
    public int? TaskId { get; set; }
    public virtual Task Task { get; set; }
    
    public int? ProjectId { get; set; }
    public virtual Project Project { get; set; }
    
    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}