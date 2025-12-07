using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CMSPortfolio.Models.External;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CMSPortfolio.Services
{
    public class SecondaryApiService
    {
        private const string CacheKey = "quote_of_the_day";

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SecondaryApiService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SecondaryApiService(
            HttpClient httpClient,
            IMemoryCache cache,
            ILogger<SecondaryApiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<QuoteOfTheDay?> GetQuoteAsync(bool forceRefresh = false)
        {
            if (!forceRefresh &&
                _cache.TryGetValue(CacheKey, out QuoteOfTheDay? cached))
            {
                return cached;
            }

            try
            {
                var response = await _httpClient.GetAsync("/random");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Quote API returned {StatusCode}", response.StatusCode);
                    return GetCachedOrFallback();
                }

                var json = await response.Content.ReadAsStringAsync();

                var dto = JsonSerializer.Deserialize<QuoteApiResponseDto>(json, JsonOptions);
                if (dto == null || string.IsNullOrWhiteSpace(dto.Content))
                    return GetCachedOrFallback();

                var quote = new QuoteOfTheDay
                {
                    Text = dto.Content!,
                    Author = string.IsNullOrWhiteSpace(dto.Author) ? "Unknown" : dto.Author!
                };

                _cache.Set(CacheKey, quote, TimeSpan.FromHours(1));

                return quote;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling quote API");
                return GetCachedOrFallback();
            }
        }

        private QuoteOfTheDay GetCachedOrFallback()
        {
            if (_cache.TryGetValue(CacheKey, out QuoteOfTheDay? cached) && cached is not null)
            {
                return cached;
            }

            // Fallback-quote om API + cache skiter sig
            var fallback = new QuoteOfTheDay
            {
                Text = "No quote available right now.",
                Author = "System"
            };

            _cache.Set(CacheKey, fallback, TimeSpan.FromMinutes(5));

            return fallback;
        }


    }
}
