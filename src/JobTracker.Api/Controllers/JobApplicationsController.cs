using JobTracker.Api.Data;
using JobTracker.Api.Models;
using JobTracker.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/job-applications")]
public class JobApplicationsController(JobTrackerDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAll([FromQuery] string? search, [FromQuery] JobApplicationStatus? status)
    {
        IQueryable<JobApplication> query = db.JobApplications.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(j =>
                EF.Functions.Like(j.Company, $"%{term}%") ||
                EF.Functions.Like(j.Position, $"%{term}%") ||
                (j.Location != null && EF.Functions.Like(j.Location, $"%{term}%")));
        }

        if (status.HasValue)
            query = query.Where(j => j.Status == status.Value);

        var items = (await query.ToListAsync())
            .OrderByDescending(j => j.UpdatedAt)
            .Select(j => j.ToDto());

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(int id)
    {
        var item = await db.JobApplications.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
        return item is null ? NotFound() : Ok(item.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create([FromBody] JobApplicationRequest request)
    {
        var validation = Validate(request);
        if (validation is not null) return validation;

        var now = DateTimeOffset.UtcNow;
        var entity = new JobApplication
        {
            Company = request.Company.Trim(),
            Position = request.Position.Trim(),
            CreatedAt = now,
            UpdatedAt = now,
        };
        entity.Apply(request);

        db.JobApplications.Add(entity);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<JobApplicationDto>> Update(int id, [FromBody] JobApplicationRequest request)
    {
        var validation = Validate(request);
        if (validation is not null) return validation;

        var entity = await db.JobApplications.FirstOrDefaultAsync(j => j.Id == id);
        if (entity is null) return NotFound();

        entity.Apply(request);
        await db.SaveChangesAsync();
        return Ok(entity.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await db.JobApplications.FirstOrDefaultAsync(j => j.Id == id);
        if (entity is null) return NotFound();

        db.JobApplications.Remove(entity);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static ActionResult? Validate(JobApplicationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Company))
            return new BadRequestObjectResult(new { message = "Company is required." });
        if (string.IsNullOrWhiteSpace(request.Position))
            return new BadRequestObjectResult(new { message = "Position is required." });
        return null;
    }
}
