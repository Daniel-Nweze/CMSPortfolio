using CMSPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CMSPortfolio.Controllers
{
    [ApiController]
    [Route("umbraco/backoffice/cmsportfolio/cache")]
    public class CacheAdminController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly GitHubApiService _gitHubService;
        private readonly SecondaryApiService _quotesService;

        // De här keysen ska matcha vad dina services använder.
        // Du sa i ground truth: "github_repositories" och "quotes_batch".
        private const string GitHubKey = "github_repositories";
        private const string QuotesKey = "quotes_batch";

        public CacheAdminController(
            IMemoryCache cache,
            GitHubApiService gitHubService,
            SecondaryApiService quotesService)
        {
            _cache = cache;
            _gitHubService = gitHubService;
            _quotesService = quotesService;
        }

        [HttpGet("status")]
        public ActionResult<ApiCacheState> GetStatus()
        {
            var state = new ApiCacheState
            {
                GitHubHasData = _cache.TryGetValue(GitHubKey, out _),
                QuotesHasData = _cache.TryGetValue(QuotesKey, out _),
                GitHubLastUpdated = null, // Kan förbättras senare
                QuotesLastUpdated = null  // Kan förbättras senare
            };

            return Ok(state);
        }

        [HttpPost("clear")]
        public IActionResult Clear()
        {
            _cache.Remove(GitHubKey);
            _cache.Remove(QuotesKey);

            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // Tvinga omhämtning från båda API:erna.
            // GitHub: vi använder forceRefresh-flaggan.
            await _gitHubService.GetRepositoriesAsync(forceRefresh: true);

            // Quotes: cachingen ligger i SecondaryApiService,
            // metoden hämtar nytt om inget finns eller cache har gått ut.
            await _quotesService.GetQuotesBatchAsync();

            return NoContent();
        }

    }
}
