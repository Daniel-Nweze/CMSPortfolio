using CMSPortfolio.Models.External;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace CMSPortfolio.Models.Viewmodels
{
    public class PortfolioPageViewModel
    {
        public IPublishedContent Page { get; }
        public IReadOnlyList<GitHubRepository> GitHubRepositories { get; }

        public PortfolioPageViewModel(
            IPublishedContent page,
            IReadOnlyList<GitHubRepository> gitHubRepositories)
        {
            Page = page;
            GitHubRepositories = gitHubRepositories;
        }
    }
}
