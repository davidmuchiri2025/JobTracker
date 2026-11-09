using JobTracker.Shared;

namespace JobTracker.Api.Models;

public class JobApplication
{
    public int Id { get; set; }
    public required string Company { get; set; }
    public required string Position { get; set; }
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Wishlist;
    public DateOnly? AppliedDate { get; set; }
    public string? Location { get; set; }
    public string? JobUrl { get; set; }
    public string? SalaryRange { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public JobApplicationDto ToDto() => new()
    {
        Id = Id,
        Company = Company,
        Position = Position,
        Status = Status,
        AppliedDate = AppliedDate,
        Location = Location,
        JobUrl = JobUrl,
        SalaryRange = SalaryRange,
        Notes = Notes,
        CreatedAt = CreatedAt,
        UpdatedAt = UpdatedAt
    };

    public void Apply(JobApplicationRequest request)
    {
        Company = request.Company.Trim();
        Position = request.Position.Trim();
        Status = request.Status;
        AppliedDate = request.AppliedDate;
        Location = request.Location?.Trim();
        JobUrl = request.JobUrl?.Trim();
        SalaryRange = request.SalaryRange?.Trim();
        Notes = request.Notes?.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
