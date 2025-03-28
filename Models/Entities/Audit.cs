namespace TImesheet_TEST.Models;

public abstract class Audit
{
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsDeleted { get; set; } = false;
}