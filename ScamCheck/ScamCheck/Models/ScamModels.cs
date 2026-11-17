namespace ScamCheck.Models;

public sealed class MaliciousDomain
{
    public int Id { get; set; }
    public required string Domain { get; set; }
    public string Category { get; set; } = "phishing";
    public string? BrandImpersonated { get; set; }
    public string? SourceNote { get; set; }
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
}

public sealed class ScamCase
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Channel { get; set; }
    public string? BrandImpersonated { get; set; }
    public required string RedFlags { get; set; }
    public DateTime AddedUtc { get; set; } = DateTime.UtcNow;
}

public sealed class ChecklistQuestion
{
    public int Id { get; set; }
    public int Order { get; set; }
    public required string Question { get; set; }
    public int Weight { get; set; } = 1;
    public required string FixAdvice { get; set; }
}
