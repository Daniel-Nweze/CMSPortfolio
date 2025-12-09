using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace CMSPortfolio.Data
{
    [TableName("ContactSubmissions")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class ContactSubmission
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name"), Length(200)]
        public string Name { get; set; } = string.Empty;

        [Column("Email"), Length(200)]
        public string Email { get; set; } = string.Empty;

        [Column("Subject"), Length(500)]
        public string Subject { get; set; } = string.Empty;


        [Column("Message")]
        public string Message { get; set; } = string.Empty;

        [Column("CreatedUtc")]
        public DateTime CreatedUtc { get; set; }
    }
}
