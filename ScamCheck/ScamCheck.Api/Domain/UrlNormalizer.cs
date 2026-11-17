namespace ScamCheck.Api.Domain;

public static class UrlNormalizer
{
    public static string? Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || !Uri.TryCreate(input.Trim(), UriKind.Absolute, out var uri) || string.IsNullOrWhiteSpace(uri.Host)) return null;
        if (uri.Scheme is not ("http" or "https")) return null;
        var host = uri.Host.TrimEnd('.').ToLowerInvariant();
        return host.StartsWith("www.", StringComparison.Ordinal) ? host[4..] : host;
    }
}
