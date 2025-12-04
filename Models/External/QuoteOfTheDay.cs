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
        public required QuoteResult[] Results { get; set; }
    }

    public class QuoteResult
    {
        [JsonPropertyName("content")]
        public required string Content { get; set; }
        [JsonPropertyName("author")]
        public required string Author { get; set; }
    }

}
