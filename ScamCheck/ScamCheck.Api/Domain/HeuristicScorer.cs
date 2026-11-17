namespace ScamCheck.Api.Domain;

public static class HeuristicScorer
{
    private static readonly string[] SuspiciousTlds = [".xyz", ".lol", ".mom", ".top", ".click", ".cyou", ".shop"];
    private static readonly string[] Brands = ["takealot", "dstv", "sars", "saa", "standardbank", "fnb", "absa", "nedbank", "capitec", "vodacom", "mtn", "telkom"];
    private static readonly HashSet<string> OfficialHosts = ["takealot.com", "dstv.com", "sars.gov.za", "flysaa.com", "standardbank.co.za", "fnb.co.za", "absa.co.za", "nedbank.co.za", "capitecbank.co.za", "vodacom.co.za", "mtn.co.za", "telkom.co.za"];
    public static int Score(string normalizedHost)
    {
        var host = normalizedHost.ToLowerInvariant(); var score = 0;
        if (SuspiciousTlds.Any(host.EndsWith)) score++;
        if (Brands.Any(host.Contains) && !OfficialHosts.Contains(host)) score++;
        if (host.Contains("xn--")) score++;
        if (host.Count(c => c == '-') >= 3) score++;
        if (host.Length > 40) score++;
        return Math.Min(score, 5);
    }
}
