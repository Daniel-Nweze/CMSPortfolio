using CMSPortfolio.Models.External;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace CMSPortfolio.Models.Viewmodels
{
    public class PortfolioPageViewModel : ContentModel
    {
        public IReadOnlyList<GitHubRepository> GitHubRepositories { get; }

        public PortfolioPageViewModel(
            IPublishedContent content,
            IReadOnlyList<GitHubRepository> gitHubRepositories)
            : base(content)
        {
            GitHubRepositories = gitHubRepositories;
        }

        public IPublishedContent Page => Content;
    }
}
