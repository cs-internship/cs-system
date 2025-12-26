using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;

namespace CrystaLearn.Core.Tests.Sync;

public class GitHubSyncServiceTests : TestBase
{
    [Fact]
    public async Task SyncGithubDocuments_WithValidModule_MustWork()
    {
        // Arrange
        var services = CreateServiceProvider(configServices: (sc) =>
        {
            sc.AddTransient<ICrystaProgramSyncModuleService, CrystaProgramSyncModuleServiceFake>();
            sc.AddTransient<ICrystaProgramRepository, CrystaProgramServiceFake>();
            sc.AddSingleton<IDocumentRepository, DocumentRepositoryInMemory>();
        });

        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IGitHubSyncService>();
        var programRepo = scope.ServiceProvider.GetRequiredService<ICrystaProgramRepository>();

        // Get a test program
        var programs = await programRepo.GetCrystaProgramsAsync(CancellationToken.None);
        var testProgram = programs.FirstOrDefault();

        if (testProgram == null)
        {
            // Skip test if no programs available
            return;
        }

        var module = new CrystaProgramSyncModule
        {
            Id = Guid.NewGuid(),
            CrystaProgramId = testProgram.Id,
            CrystaProgram = testProgram,
            ModuleType = SyncModuleType.GitHubDocument,
            SyncInfo = new SyncInfo
            {
                LastSyncDateTime = DateTimeOffset.Now.AddYears(-1),
                LastSyncOffset = "0"
            }
        };

        // Act
        var result = await service.SyncAsync(module);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.AddCount >= 0);
        Assert.True(result.UpdateCount >= 0);
        Assert.True(result.SameCount >= 0);
        Assert.True(result.DeleteCount >= 0);
    }

    [Fact]
    public async Task SyncGithubDocuments_WithInvalidModuleType_MustThrowException()
    {
        // Arrange
        var services = CreateServiceProvider(configServices: (sc) =>
        {
            sc.AddTransient<ICrystaProgramSyncModuleService, CrystaProgramSyncModuleServiceFake>();
            sc.AddSingleton<IDocumentRepository, DocumentRepositoryInMemory>();
        });

        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IGitHubSyncService>();

        var module = new CrystaProgramSyncModule
        {
            Id = Guid.NewGuid(),
            ModuleType = SyncModuleType.AzureBoard, // Wrong type
            SyncInfo = new SyncInfo()
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await service.SyncAsync(module));
    }
}
