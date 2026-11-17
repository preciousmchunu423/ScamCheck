using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScamCheck.Data;

namespace ScamCheck.Controllers;

[ApiController]
[Route("api/scam")]
public sealed class ScamApiController(AppDbContext db) : ControllerBase
{
    [HttpPost("check-link")]
    public async Task<IActionResult> CheckLink([FromBody] LinkRequest request)
    {
        var host = Normalize(request.Url);
        if (host is null) return BadRequest(new { verdict = "invalid" });
        var hit = await db.MaliciousDomains.AsNoTracking().FirstOrDefaultAsync(x => x.Domain == host);
        if (hit is not null)
            return Ok(new { verdict = "malicious", category = hit.Category, brand = hit.BrandImpersonated, advice = "Do not click. This domain matches a known phishing campaign." });
        var suspicious = host.Contains("renewal") || host.Contains("discount") || host.Contains("secure");
        return Ok(new { verdict = suspicious ? "suspicious" : "unknown", advice = suspicious ? "This domain has traits common in scam links. Treat with caution." : "Not in our blacklist does not mean safe. Verify through an official channel." });
    }

    [HttpGet("cases")]
    public async Task<IActionResult> Cases([FromQuery] string? q, [FromQuery] string? brand)
    {
        var query = db.ScamCases.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Title.Contains(q) || x.Description.Contains(q) || x.RedFlags.Contains(q));
        if (!string.IsNullOrWhiteSpace(brand) && brand != "All brands") query = query.Where(x => x.BrandImpersonated == brand);
        return Ok(await query.OrderByDescending(x => x.AddedUtc).ToListAsync());
    }

    [HttpGet("checklist")]
    public async Task<IActionResult> GetChecklist()
    {
        return Ok(await db.ChecklistQuestions.AsNoTracking().OrderBy(x => x.Order).ToListAsync());
    }

    [HttpPost("submit-checklist")]
    public async Task<IActionResult> SubmitChecklist([FromBody] ChecklistRequest request)
    {
        if (request.Answers is null) return BadRequest(new { error = "No answers provided." });

        var questions = await db.ChecklistQuestions.AsNoTracking().OrderBy(x => x.Order).ToListAsync();
        var maximum = questions.Sum(x => x.Weight);
        var risk = 0;
        var fixes = new List<string>();

        foreach (var question in questions)
        {
            if (!request.Answers.TryGetValue(question.Id, out var answer)) continue;

            // Questions 1-3 describe risky conditions when answered Yes;
            // the remaining questions describe protective controls when answered No.
            var isRisk = question.Order <= 3 ? answer : !answer;
            if (isRisk)
            {
                risk += question.Weight;
                fixes.Add(question.FixAdvice);
            }
        }

        var riskScore = maximum == 0 ? 0 : (int)Math.Round(risk * 100.0 / maximum);
        return Ok(new { riskScore, fixes });
    }

    private static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var candidate = value.Trim();
        if (!candidate.Contains("://")) candidate = "https://" + candidate;
        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri) || string.IsNullOrWhiteSpace(uri.Host)) return null;
        var host = uri.Host.ToLowerInvariant();
        return host.StartsWith("www.", StringComparison.Ordinal) ? host[4..] : host;
    }

    public sealed record LinkRequest(string? Url);
    public sealed record ChecklistRequest(Dictionary<int, bool>? Answers);
}
