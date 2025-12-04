using CMSPortfolio.Models.Viewmodels;
using CMSPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CMSPortfolio.Controllers
{
    public class PortfolioPageController : RenderController
    {
        private readonly GitHubApiService _gitHubApiService;
        private readonly SecondaryApiService _secondaryApiService;

        public PortfolioPageController(
            ILogger<PortfolioPageController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            GitHubApiService gitHubApiService,
            SecondaryApiService secondaryApiService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _gitHubApiService = gitHubApiService;
            _secondaryApiService = secondaryApiService;
        }

        public override IActionResult Index()
        {
            var repos = _gitHubApiService.GetRepositoriesAsync().GetAwaiter().GetResult();
            var quote = _secondaryApiService.GetQuotesBatchAsync().GetAwaiter().GetResult();

            var model = new PortfolioPageViewModel(CurrentPage!, repos);    

            return CurrentTemplate(model);
        }
    }
}
