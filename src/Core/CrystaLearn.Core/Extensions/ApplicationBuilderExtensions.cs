using CrystaLearn.Core.Data;
using CrystaLearn.Core.Services;
using CrystaLearn.Core.Services.AzureBoard;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.GitHub;
using CrystaLearn.Core.Services.Sync;
using Microsoft.Extensions.Hosting;
using Octokit;

namespace CrystaLearn.Core.Extensions;
public static class ApplicationBuilderExtensions
{
    public static void AddCrystaServices(this IHostApplicationBuilder builder)
    {
        var env = builder.Environment;
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddPooledDbContextFactory<AppDbContext>(AddDbContext);
        services.AddDbContextPool<AppDbContext>(AddDbContext);

        void AddDbContext(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging(env.IsDevelopment())
                .EnableDetailedErrors(env.IsDevelopment());

            options.UseNpgsql(configuration.GetConnectionString("PostgresConnectionString"));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        builder.AddGitHubClient();

        services.AddTransient<IDocumentRepository, DocumentRepositoryDirectGitHub>();
        
        services.AddTransient<ICrystaProgramRepository, CrystaProgramService>();
        services.AddTransient<ICrystaDocumentService, CrystaDocumentService>();
        services.AddTransient<IGitHubService, GitHubService>();
        services.AddTransient<IAzureBoardService, AzureBoardService>();
        services.AddTransient<ICrystaProgramSyncService, CrystaProgramSyncService>();
        services.AddTransient<IAzureBoardSyncService, AzureBoardSyncService>();
        services.AddTransient<IGithubSyncService, GithubSyncService>();
        services.AddTransient<ICrystaProgramSyncModuleService, CrystaProgramSyncModuleService>();
        services.AddTransient<ICrystaTaskService, CrystaTaskService>();
    }

    private static void AddGitHubClient(this IHostApplicationBuilder builder)
    {
        var env = builder.Environment;
        var services = builder.Services;
        var configuration = builder.Configuration;

        var productHeaderValue = new ProductHeaderValue("CS-System");
        var gitHubToken = configuration["GitHub:GitHubAccessToken"] ?? "";
        var tokenAuth = new Credentials(gitHubToken);
        var client = new GitHubClient(productHeaderValue)
        {
            Credentials = tokenAuth
        };

        services.AddSingleton(client);

    }
}
