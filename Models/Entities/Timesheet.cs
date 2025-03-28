namespace TImesheet_TEST.Models;

public class Timesheet : Audit
{
    public int TimesheetId { get; set; }
    public string? WorkType { get; set; }
    public bool IsCharged { get; set; }
    public string? WorkingTime { get; set; }
    public DateTime? TimeEntry { get; set; }
    public string? TimesheetNote { get; set; }
    public string? TimesheetStatus { get; set; }
    public string? DecidedBy { get; set; }
    public DateTime? DecidedOn { get; set; }
    
    public int? UserId { get; set; }
    public virtual User User { get; set; }
    
    public int? TaskProjectId { get; set; }
    public virtual TaskProject TaskProject { get; set; }
    
}