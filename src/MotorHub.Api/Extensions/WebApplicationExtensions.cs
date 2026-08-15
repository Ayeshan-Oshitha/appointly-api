using MotorHub.Infrastructure.Identity;
using MotorHub.Infrastructure.Persistence;
using MotorHub.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        /// <summary>Used only when DemoData:SellerPassword is not configured.</summary>
        private const string FallbackDemoSellerPassword = "Demo@12345";

        /// <summary>
        /// Brings the database up to date and seeds it, before the app serves any traffic.
        /// Reference data is seeded in every environment; demo data only in Development.
        /// </summary>
        public static async Task InitializeDatabaseAsync(
            this WebApplication app,
            CancellationToken cancellationToken = default)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILoggerFactory>()
                                 .CreateLogger("MotorHub.Api.DatabaseInitializer");

            try
            {
                var dbContext = services.GetRequiredService<MotorHubDbContext>();

                await ApplyMigrationsAsync(app, dbContext, logger, cancellationToken);

                // Roles first: DemoUserSeeder assigns roles, so they must already exist.
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                await RoleSeeder.SeedAsync(roleManager);
                logger.LogInformation("Roles seeded.");

                // Reference data - required in every environment.
                await LocationSeeder.SeedAsync(dbContext);
                await BrandModelSeeder.SeedAsync(dbContext);
                logger.LogInformation("Reference data seeded.");

                if (!app.Environment.IsDevelopment())
                {
                    logger.LogInformation(
                        "Skipping demo data seeding in environment {Environment}.",
                        app.Environment.EnvironmentName);
                    return;
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var demoPassword = app.Configuration["DemoData:SellerPassword"];

                if (string.IsNullOrWhiteSpace(demoPassword))
                {
                    demoPassword = FallbackDemoSellerPassword;

                    logger.LogWarning(
                        "'DemoData:SellerPassword' is not configured; using the built-in development " +
                        "password. Set it with: dotnet user-secrets set \"DemoData:SellerPassword\" " +
                        "\"<password>\" --project src/MotorHub.Api");
                }

                var demoSeller = await DemoUserSeeder.SeedAsync(
                    dbContext, userManager, demoPassword, logger, cancellationToken);

                await AdvertisementSeeder.SeedAsync(
                    dbContext, demoSeller.Id, logger, cancellationToken);

                logger.LogInformation("Demo data seeded (seller: {Email}).", DemoUserSeeder.SellerEmail);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database initialization failed; the application will not start.");
                throw;
            }
        }

        private static async Task ApplyMigrationsAsync(
            WebApplication app,
            MotorHubDbContext dbContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            // Applying migrations at startup is convenient for a single-instance dev API, but
            // carries real costs elsewhere: concurrent instances contend on the migration lock,
            // destructive DDL runs without review, and the app's DB user needs DDL rights
            // permanently. Hence: on by default in Development, off everywhere else.
            var autoMigrate = app.Configuration.GetValue(
                "Database:AutoMigrateOnStartup",
                app.Environment.IsDevelopment());

            var knownMigrations = dbContext.Database.GetMigrations().ToList();

            if (knownMigrations.Count == 0)
            {
                throw new InvalidOperationException(
                    "No EF Core migrations exist in the assembly, so the database schema cannot be " +
                    "created. Generate one first: dotnet ef migrations add InitialCreate " +
                    "--project src/MotorHub.Infrastructure --startup-project src/MotorHub.Api");
            }

            if (!autoMigrate)
            {
                var pending = (await dbContext.Database
                    .GetPendingMigrationsAsync(cancellationToken)).ToList();

                if (pending.Count > 0)
                {
                    throw new InvalidOperationException(
                        $"The database has {pending.Count} pending migration(s) " +
                        $"({string.Join(", ", pending)}) and Database:AutoMigrateOnStartup is " +
                        "disabled. Apply them before starting the API.");
                }

                logger.LogInformation("Automatic migration disabled; schema is already up to date.");
                return;
            }

            logger.LogInformation("Applying pending database migrations...");
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database schema is up to date.");
        }
    }
}
