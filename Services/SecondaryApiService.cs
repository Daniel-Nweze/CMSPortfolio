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
        private const string CacheKey = "quotes_batch";

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


        public async Task<List<QuoteOfTheDay>> GetQuotesBatchAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<QuoteOfTheDay>? cached))
                return cached!;

            try
            {
                // Hämta 20 citat i ett paket
                var response = await _httpClient.GetAsync("https://zenquotes.io/api/quotes");

                if (!response.IsSuccessStatusCode)
                    return cached ?? new List<QuoteOfTheDay>();

                var json = await response.Content.ReadAsStringAsync();
                var arr = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

                if (arr == null) return cached ?? new List<QuoteOfTheDay>();

                var list = arr.Take(20)
                              .Select(x => new QuoteOfTheDay
                              {
                                  Text = x["q"],
                                  Author = x["a"]
                              })
                              .ToList();

                _cache.Set(CacheKey, list, TimeSpan.FromMinutes(10));

                return list;
            }
            catch
            {
                return cached ?? new List<QuoteOfTheDay>();
            }
        }

        private QuoteOfTheDay? GetCachedOrNull()
        {
            return _cache.TryGetValue(CacheKey, out QuoteOfTheDay? cached)
                ? cached
                : null;
        }
    }
}
