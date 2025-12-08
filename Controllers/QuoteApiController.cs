using CMSPortfolio.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSPortfolio.Controllers
{
    [ApiController]
    [Route("api/quote")]
    public class QuoteApiController : ControllerBase
    {
        private readonly SecondaryApiService _secondary;

        public QuoteApiController(SecondaryApiService secondary)
        {
            _secondary = secondary;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _secondary.GetQuotesBatchAsync();
            return Ok(list);
        }
    }
}
