using Microsoft.EntityFrameworkCore;

namespace ScamCheck.Api.Data;

public class MaliciousDomain { public int Id { get; set; } public string Domain { get; set; } = ""; public string Category { get; set; } = ""; public string? BrandImpersonated { get; set; } public DateTime AddedUtc { get; set; } public string? SourceNote { get; set; } }
public class ScamCase { public int Id { get; set; } public string Title { get; set; } = ""; public string Channel { get; set; } = ""; public string BrandImpersonated { get; set; } = ""; public string Description { get; set; } = ""; public string RedFlags { get; set; } = ""; public DateTime AddedUtc { get; set; } }
public class ChecklistQuestion { public int Id { get; set; } public string Text { get; set; } = ""; public int Weight { get; set; } public string FixAdvice { get; set; } = ""; public int Order { get; set; } }
public class ChecklistSubmission { public int Id { get; set; } public DateTime SubmittedUtc { get; set; } public int RiskScore { get; set; } public string AnswersJson { get; set; } = ""; }

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<MaliciousDomain> MaliciousDomains => Set<MaliciousDomain>();
    public DbSet<ScamCase> ScamCases => Set<ScamCase>();
    public DbSet<ChecklistQuestion> ChecklistQuestions => Set<ChecklistQuestion>();
    public DbSet<ChecklistSubmission> ChecklistSubmissions => Set<ChecklistSubmission>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<MaliciousDomain>().HasIndex(x => x.Domain).IsUnique();
}
