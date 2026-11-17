using ScamCheck.Api.Domain;

namespace ScamCheck.Tests;

public class DomainTests
{
    [Theory]
    [InlineData("HTTPS://WWW.Takealot-sa.xyz/path?q=1", "takealot-sa.xyz")]
    [InlineData("https://www.takealot-sa.xyz/", "takealot-sa.xyz")]
    [InlineData("https://takealot-sa.xyz", "takealot-sa.xyz")]
    public void NormalizesHost(string input, string expected) => Assert.Equal(expected, UrlNormalizer.Normalize(input));

    [Theory]
    [InlineData("")]
    [InlineData("not a url")]
    [InlineData("http://")]
    public void InvalidInputReturnsNull(string input) => Assert.Null(UrlNormalizer.Normalize(input));

    [Fact] public void TakealotSuspiciousDomainScoresExactlyTwo() => Assert.Equal(2, HeuristicScorer.Score("takealot-sa.xyz"));
    [Fact] public void PunycodeHostScoresExactlyOne() => Assert.Equal(1, HeuristicScorer.Score("xn--example.test"));
    [Fact] public void OfficialTakealotScoresExactlyZero() => Assert.Equal(0, HeuristicScorer.Score("takealot.com"));
}
