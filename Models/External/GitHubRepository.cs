using System;
using System.Text.Json.Serialization;

namespace CMSPortfolio.Models.External
{
    public class GitHubRepository
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Stars { get; set; }
        public DateTimeOffset LastPushDate { get; set; }
    }

    public class GitHubRepositoryDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("stargazers_count")]
        public int StargazersCount { get; set; }

        [JsonPropertyName("pushed_at")]
        public DateTimeOffset? PushedAt { get; set; }
    }
}
