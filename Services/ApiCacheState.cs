namespace CMSPortfolio.Services
{
    public class ApiCacheState
    {
        public bool GitHubHasData { get; set; }
        public bool QuotesHasData { get; set; }

        public DateTimeOffset? GitHubLastUpdated { get; set; }
        public DateTimeOffset? QuotesLastUpdated { get; set; }
    }
}
