using CMSPortfolio.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMSPortfolio.Controllers
{
    [ApiController]
    [Route("umbraco/backoffice/cmsportfolio/contact")]
    public class ContactAdminController : ControllerBase
    {
        private readonly ContactSubmissionRepository _repository;

        public ContactAdminController(ContactSubmissionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var items = _repository.GetAll();
            return Ok(items);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
