namespace JobTracker.Shared;

public class JobApplicationDto
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Wishlist;
    public DateOnly? AppliedDate { get; set; }
    public string? Location { get; set; }
    public string? JobUrl { get; set; }
    public string? SalaryRange { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class JobApplicationRequest
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Wishlist;
    public DateOnly? AppliedDate { get; set; }
    public string? Location { get; set; }
    public string? JobUrl { get; set; }
    public string? SalaryRange { get; set; }
    public string? Notes { get; set; }
}
