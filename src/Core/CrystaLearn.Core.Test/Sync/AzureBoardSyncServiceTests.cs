using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services;
using CrystaLearn.Core.Services.AzureBoard;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;

namespace CrystaLearn.Core.Tests.Sync;

public class AzureBoardSyncServiceTests : TestBase
{
    [Fact]
    public async Task SyncAzureBoard_WithFakeData_MustWork()
    {
        // Arrange
        var services = CreateServiceProvider(configServices: (sc) =>
        {
            sc.AddTransient<ICrystaProgramSyncModuleRepository, CrystaProgramSyncModuleRepositoryFake>();
        });

        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAzureBoardSyncService>();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var organization = "cs-internship";
        var pat = configuration["AzureDevOps:PersonalAccessToken"]
                  ?? throw new Exception("No PAT provided.");

        var project = "CS Internship Program";

        var config = new AzureBoardSyncConfig
        {
            Organization = organization,
            PersonalAccessToken = pat,
            Project = project,
            WorkItemChangedFromDateTime = DateTimeOffset.Now.AddDays(-10)
        };

        var module = new CrystaProgramSyncModule
        {
            ModuleType = SyncModuleType.AzureBoard,
            SyncConfig = System.Text.Json.JsonSerializer.Serialize(config),
            SyncInfo = new SyncInfo
            {
                LastSyncDateTime = DateTimeOffset.Now.AddDays(-4),
                LastSyncOffset = "0"
            }
        };

        await service.SyncAsync(module,new List<int>() { 14412 });
    }

    [Fact]
    public async Task SyncAzureBoard_WithFakeRepo_MustWork()
    {
        // Arrange
        var services = CreateServiceProvider(configServices: (sc) =>
        {
            sc.AddTransient<ICrystaProgramSyncModuleRepository, CrystaProgramSyncModuleRepositoryFake>();
        });

        var moduleService = services.GetRequiredService<ICrystaProgramSyncModuleRepository>();
        var syncService = services.GetRequiredService<IAzureBoardSyncService>();

        var modules = await moduleService.GetSyncModulesAsync(default);

        foreach (var module in modules)
        {
            await syncService.SyncAsync(module);
        }
    }
}

//AzureBoardService Test
//AzureSyncService Test
// CrystaTaskRepository -> CrystaTaskService  -> Documented
// 
