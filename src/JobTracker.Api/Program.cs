using JobTracker.Api.Data;
using JobTracker.Api.Models;
using JobTracker.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=jobtracker.db";
const string dataSourcePrefix = "Data Source=";
if (connectionString.StartsWith(dataSourcePrefix, StringComparison.OrdinalIgnoreCase))
{
    var dataSource = connectionString[dataSourcePrefix.Length..].Trim();
    var directory = Path.GetDirectoryName(dataSource);
    if (!string.IsNullOrWhiteSpace(directory))
    {
        Directory.CreateDirectory(directory);
    }
}

builder.Services.AddDbContext<JobTrackerDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("web", policy =>
    {
        policy.WithOrigins(
                builder.Configuration["Cors:WebOrigin"] ?? "http://localhost:5257")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JobTrackerDbContext>();
    await db.Database.EnsureCreatedAsync();

    if (!await db.JobApplications.AnyAsync())
    {
        var now = DateTimeOffset.UtcNow;
        db.JobApplications.AddRange(
            new JobApplication
            {
                Company = "Acme Corp",
                Position = "Backend Engineer",
                Status = JobApplicationStatus.Applied,
                AppliedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)),
                Location = "Remote",
                Notes = "Referral from former colleague. Follow up next week.",
                CreatedAt = now.AddDays(-7),
                UpdatedAt = now.AddDays(-1)
            },
            new JobApplication
            {
                Company = "Contoso",
                Position = "Full Stack .NET Developer",
                Status = JobApplicationStatus.Interviewing,
                AppliedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14)),
                Location = "Nairobi, KE (Hybrid)",
                JobUrl = "https://example.com/jobs/contoso-dotnet",
                SalaryRange = "Competitive",
                Notes = "Technical interview scheduled.",
                CreatedAt = now.AddDays(-14),
                UpdatedAt = now.AddDays(-2)
            });
        await db.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("web");
app.UseAuthorization();
app.MapControllers();

app.Run();
