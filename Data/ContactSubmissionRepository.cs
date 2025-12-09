using System;
using System.Collections.Generic;
using Umbraco.Cms.Infrastructure.Scoping;

namespace CMSPortfolio.Data
{
    public class ContactSubmissionRepository
    {
        private readonly IScopeProvider _scopeProvider;

        public ContactSubmissionRepository(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public void Insert(ContactSubmission submission)
        {
            using var scope = _scopeProvider.CreateScope(autoComplete: true);

            submission.CreatedUtc = DateTime.UtcNow;

            scope.Database.Insert(submission);
            // autoComplete: true gör scope.Complete() åt dig
        }

        public IEnumerable<ContactSubmission> GetAll()
        {
            using var scope = _scopeProvider.CreateScope(autoComplete: true);

            return scope.Database.Fetch<ContactSubmission>();
        }

        public void Delete(int id)
        {
            using var scope = _scopeProvider.CreateScope();
            var db = scope.Database;

            db.Delete<ContactSubmission>(id);

            scope.Complete();
        }
    }
}
