using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using CrystaLearn.Core.Data;

namespace CrystaLearn.Tests;

[TestClass]
public partial class TestsInitializer
{
    [AssemblyInitialize]
    public static async Task Initialize(TestContext testContext)
    {
        await using var testServer = new AppTestServer();

        await testServer.Build().Start();

        await InitializeDatabase(testServer);
    }

    private static async Task InitializeDatabase(AppTestServer testServer)
    {
        if (testServer.WebApp.Environment.IsDevelopment())
        {
            await using var scope = testServer.WebApp.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
            }
            else if ((await dbContext.Database.GetAppliedMigrationsAsync()).Any() is false)
            {
                throw new InvalidOperationException("No migrations have been added. Please ensure that migrations are added before running tests.");
            }
        }
    }
}
