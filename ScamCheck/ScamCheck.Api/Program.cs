using System.Text.Json;
using ScamCheck.Api.Data;
using ScamCheck.Api.Domain;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=scamcheck.db;Cache=Shared;Default Timeout=30;Synchronous=Normal"));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.OpenConnectionAsync();
    try
    {
        await db.Database.EnsureCreatedAsync();
        await db.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");
        await db.Database.ExecuteSqlRawAsync("PRAGMA synchronous=NORMAL;");
    }
    finally
    {
        await db.Database.CloseConnectionAsync();
    }
    await SeedData.InitializeAsync(db);
}

app.MapPost("/api/check-link", async (LinkRequest request, AppDbContext db, ILogger<Program> logger) =>
{
    var host = UrlNormalizer.Normalize(request.Url ?? "");
    if (host is null) return Results.BadRequest(new { verdict = "invalid" });
    var hit = await db.MaliciousDomains.AsNoTracking().FirstOrDefaultAsync(x => x.Domain == host);
    if (hit is not null)
    {
        logger.LogInformation("Link check {Host} verdict {Verdict}", host, "malicious");
        return Results.Ok(new { verdict = "malicious", category = hit.Category, brand = hit.BrandImpersonated, advice = "Do not open the link or provide information. Report and delete the message." });
    }
    var score = HeuristicScorer.Score(host); var verdict = score >= 2 ? "suspicious" : "unknown";
    logger.LogInformation("Link check {Host} verdict {Verdict}", host, verdict);
    return Results.Ok(new { verdict, score, advice = score >= 2 ? "Treat this link with caution. Verify through an official app or website." : "Not in our blacklist does not mean safe. Verify through an official channel." });
});

app.MapGet("/api/cases", async (string? q, string? brand, AppDbContext db) =>
{
    var query = db.ScamCases.AsNoTracking().AsQueryable();
    if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Title.Contains(q) || x.Description.Contains(q) || x.RedFlags.Contains(q));
    if (!string.IsNullOrWhiteSpace(brand)) query = query.Where(x => x.BrandImpersonated == brand);
    return Results.Ok(await query.OrderByDescending(x => x.AddedUtc).Take(100).ToListAsync());
});
app.MapGet("/api/checklist/questions", async (AppDbContext db) => Results.Ok(await db.ChecklistQuestions.AsNoTracking().OrderBy(x => x.Order).ToListAsync()));
app.MapPost("/api/checklist/submit", async (ChecklistRequest request, AppDbContext db) =>
{
    var answers = request.Answers ?? new Dictionary<string, bool>();
    var questions = await db.ChecklistQuestions.AsNoTracking().ToListAsync();
    var total = questions.Sum(x => x.Weight); var earned = questions.Where(x => answers.TryGetValue(x.Id.ToString(), out var present) && present).Sum(x => x.Weight);
    var score = total == 0 ? 0 : (int)Math.Round(earned * 100.0 / total);
    db.ChecklistSubmissions.Add(new ChecklistSubmission { SubmittedUtc = DateTime.UtcNow, RiskScore = score, AnswersJson = JsonSerializer.Serialize(answers) });
    await db.SaveChangesAsync();
    return Results.Ok(new { riskScore = score, fixes = questions.Where(x => answers.TryGetValue(x.Id.ToString(), out var present) && present).Select(x => x.FixAdvice).ToArray() });
});
app.MapPost("/api/admin/domains", async (AdminDomainRequest request, HttpRequest http, IConfiguration config, AppDbContext db) =>
{
    var expected = config["Admin:ApiKey"];
    if (string.IsNullOrWhiteSpace(expected) || http.Headers["X-Api-Key"] != expected) return Results.Unauthorized();
    var domain = UrlNormalizer.Normalize(request.Domain ?? "");
    if (domain is null) return Results.BadRequest(new { error = "Invalid domain" });
    var existing = await db.MaliciousDomains.AsNoTracking().FirstOrDefaultAsync(x => x.Domain == domain);
    if (existing is not null) return Results.Ok(new { domain = existing.Domain, inserted = false });
    db.MaliciousDomains.Add(new MaliciousDomain { Domain = domain, Category = request.Category ?? "phishing", BrandImpersonated = request.Brand, AddedUtc = DateTime.UtcNow, SourceNote = "API submission" });
    await db.SaveChangesAsync(); return Results.Ok(new { domain, inserted = true });
});
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();

public record LinkRequest(string? Url);
public record ChecklistRequest(Dictionary<string, bool>? Answers);
public record AdminDomainRequest(string? Domain, string? Category, string? Brand);
