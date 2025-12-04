using CMSPortfolio.Data;
using CMSPortfolio.Models.Viewmodels;

namespace CMSPortfolio.Services
{
    public class ContactFormService
    {
        private readonly ContactSubmissionRepository _repository;

        public ContactFormService(ContactSubmissionRepository repository)
        {
            _repository = repository;
        }

        public void Save(ContactFormViewModel model)
        {
            var record = new ContactSubmission
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message
            };

            _repository.Insert(record);
        }
    }

}
