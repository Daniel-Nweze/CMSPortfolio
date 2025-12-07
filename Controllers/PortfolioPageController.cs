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

        public PortfolioPageController(
            ILogger<PortfolioPageController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            GitHubApiService gitHubApiService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _gitHubApiService = gitHubApiService;
        }

        public override IActionResult Index()
        {
            // RenderController är sync, så vi blockerar här – duger för skolprojekt
            var repos = _gitHubApiService.GetRepositoriesAsync().GetAwaiter().GetResult();

            var vm = new PortfolioPageViewModel(CurrentPage!, repos);

            return CurrentTemplate(vm);
        }
    }
}
