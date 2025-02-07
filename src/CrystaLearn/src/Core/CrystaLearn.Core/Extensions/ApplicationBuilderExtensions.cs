﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        builder.AddGitHubClient();

        services.AddPooledDbContextFactory<AppDbContext>(AddDbContext);
        services.AddDbContextPool<AppDbContext>(AddDbContext);

        void AddDbContext(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging(env.IsDevelopment())
                   .EnableDetailedErrors(env.IsDevelopment());

            options.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString"), dbOptions =>
            {

            });
        };

        services.AddSingleton<IDocumentRepository, DocumentRepositoryInMemory>();
        services.AddTransient<ICrystaProgramRepository, CrystaProgramRepositoryFake>();
        services.AddTransient<IGitHubService, GitHubService>();
        services.AddTransient<IAzureBoardService, AzureBoardService>();
        services.AddTransient<ICrystaProgramSyncService, CrystaProgramSyncService>();
        services.AddTransient<ICrystaProgramSyncModuleRepository, CrystaProgramSyncModuleRepositoryFake>();
    }

    private static void AddGitHubClient(this IHostApplicationBuilder builder)
    {
        var env = builder.Environment;
        var services = builder.Services;
        var configuration = builder.Configuration;

        var productHeaderValue = new ProductHeaderValue("CS-System");
        var gitHubToken = configuration["GitHub:GitHubAccessToken"];
        var tokenAuth = new Credentials(gitHubToken);
        var client = new GitHubClient(productHeaderValue)
        {
            Credentials = tokenAuth
        };

        services.AddSingleton(client);

    }
}
