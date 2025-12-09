using NPoco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace CMSPortfolio.Infrastructure.Migrations;

// Registrerar notification handler
public class ContactFormMigrationComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunContactFormMigration>();
    }
}

public class RunContactFormMigration : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IMigrationPlanExecutor _migrationPlanExecutor;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly IKeyValueService _keyValueService;
    private readonly IRuntimeState _runtimeState;
    private readonly ILogger<RunContactFormMigration> _logger;

    public RunContactFormMigration(
        ICoreScopeProvider coreScopeProvider,
        IMigrationPlanExecutor migrationPlanExecutor,
        IKeyValueService keyValueService,
        IRuntimeState runtimeState,
        ILogger<RunContactFormMigration> logger)
    {
        _migrationPlanExecutor = migrationPlanExecutor;
        _coreScopeProvider = coreScopeProvider;
        _keyValueService = keyValueService;
        _runtimeState = runtimeState;
        _logger = logger;
    }

    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        if (_runtimeState.Level < RuntimeLevel.Run)
        {
            return; // Umbraco inte färdigbootat, gör inget
        }

        var plan = new MigrationPlan("ContactForm");

        plan.From(string.Empty)
            .To<CreateContactFormTable>("contactform-db");

        var upgrader = new Upgrader(plan);

        _logger.LogInformation("Running ContactForm migrations");
        // ✔ Kör async-varianten – den är *inte* obsolete
        upgrader.ExecuteAsync(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
    }
}

public class CreateContactFormTable : MigrationBase
{
    public const string TableName = "ContactSubmissions";

    public CreateContactFormTable(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        Logger.LogDebug("Running migration {MigrationStep}", "CreateContactFormTable");

        if (TableExists(TableName))
        {
            Logger.LogDebug("Table {TableName} already exists, skipping", TableName);
            return;
        }

        Create.Table<ContactSubmissionSchema>().Do();
    }

    [TableName(TableName)]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class ContactSubmissionSchema
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        [Length(200)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Name { get; set; } = string.Empty;

        [Column("Email")]
        [Length(200)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Email { get; set; } = string.Empty;

        [Column("Subject")]
        [Length(500)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Subject { get; set; } = string.Empty;

        [Column("Message")]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public string Message { get; set; } = string.Empty;

        [Column("CreatedUtc")]
        [NullSetting(NullSetting = NullSettings.NotNull)]
        public DateTime CreatedUtc { get; set; }
    }
}
