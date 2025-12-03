using CMSPortfolio.Models.Viewmodels;
using CMSPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace CMSPortfolio.Controllers
{
    public class ContactFormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory umbracoDatabaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        ContactFormService contactFormService) : SurfaceController(umbracoContextAccessor, umbracoDatabaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        private readonly ContactFormService _contactFormService = contactFormService;

        [HttpPost]
        public IActionResult Submit(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ContactFormError"] = "Please fill in all required fields.";
                return CurrentUmbracoPage(); // stannar på samma sida, visar fel
            }

            try
            {
                _contactFormService.Save(model);
                TempData["ContactFormSuccess"] = "Thank you for your message.";
            }
            catch
            {
                TempData["ContactFormError"] = "Something went wrong. Please try again later.";
            }

            return CurrentUmbracoPage();
        }
    }   
}
