using System.Text.Json.Serialization;

namespace CMSPortfolio.Models.External
{
    public class QuoteOfTheDay
    {
        public string Text { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }

    public class QuoteApiResponseDto
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }
    }
}
