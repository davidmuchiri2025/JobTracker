using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Data;

public class JobTrackerDbContext(DbContextOptions<JobTrackerDbContext> options) : DbContext(options)
{
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Company).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.JobUrl).HasMaxLength(500);
            entity.Property(e => e.SalaryRange).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(4000);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
        });
    }
}
