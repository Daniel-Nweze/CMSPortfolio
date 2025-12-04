using CMSPortfolio.Models.External;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CMSPortfolio.Services
{
    public class GitHubApiService
    {
        private const string CacheKey = "github_repositories";
        private const string Username = "Daniel-Nweze"; // ditt GitHub-username

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GitHubApiService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public GitHubApiService(
            HttpClient httpClient,
            IMemoryCache cache,
            ILogger<GitHubApiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IReadOnlyList<GitHubRepository>> GetRepositoriesAsync(bool forceRefresh = false)
        {
            if (!forceRefresh &&
                _cache.TryGetValue(CacheKey, out IReadOnlyList<GitHubRepository>? cached))
            {
                return cached!;
            }

            try
            {
                var response = await _httpClient.GetAsync(
                    $"/users/{Username}/repos?sort=pushed&direction=desc&per_page=8");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GitHub API returned {StatusCode}", response.StatusCode);
                    return GetCachedOrEmpty();
                }

                var json = await response.Content.ReadAsStringAsync();

                var dtoList = JsonSerializer.Deserialize<List<GitHubRepositoryDto>>(json, JsonOptions)
                             ?? new List<GitHubRepositoryDto>();

                var mapped = dtoList
                    .Select(dto => new GitHubRepository
                    {
                        Name = dto.Name ?? string.Empty,
                        Description = dto.Description ?? string.Empty,
                        HtmlUrl = dto.HtmlUrl ?? string.Empty,
                        Language = dto.Language ?? "Unknown",
                        Stars = dto.StargazersCount,
                        LastPushDate = dto.PushedAt ?? DateTimeOffset.MinValue
                    })
                    .OrderByDescending(r => r.LastPushDate)
                    .ToList();

                _cache.Set(CacheKey, mapped, TimeSpan.FromMinutes(30));

                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling GitHub API");
                return GetCachedOrEmpty();
            }
        }

        private IReadOnlyList<GitHubRepository> GetCachedOrEmpty()
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<GitHubRepository>? cached))
                return cached!;

            return Array.Empty<GitHubRepository>();
        }
    }
}
