using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CMSPortfolio.Services
{
    public class ApiCacheRefreshService : BackgroundService
    {
        private readonly GitHubApiService _gitHubApiService;
        private readonly SecondaryApiService _secondaryApiService;
        private readonly ILogger<ApiCacheRefreshService> _logger;

        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);

        public ApiCacheRefreshService(
            GitHubApiService gitHubApiService,
            SecondaryApiService secondaryApiService,
            ILogger<ApiCacheRefreshService> logger)
        {
            _gitHubApiService = gitHubApiService;
            _secondaryApiService = secondaryApiService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RefreshAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(Interval, stoppingToken);
                    await RefreshAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // shutdown
                }
            }
        }

        private async Task RefreshAsync(CancellationToken token)
        {
            try
            {
                _logger.LogInformation("Refreshing API caches...");

                await _gitHubApiService.GetRepositoriesAsync(forceRefresh: true);
                await _secondaryApiService.GetQuoteAsync(forceRefresh: true);

                _logger.LogInformation("API caches refreshed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing API caches");
            }
        }
    }
}
