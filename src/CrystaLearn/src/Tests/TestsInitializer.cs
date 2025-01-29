using Microsoft.EntityFrameworkCore;
using CrystaLearn.Server.Api.Data;
using Microsoft.Extensions.Hosting;

namespace CrystaLearn.Tests;

[TestClass]
public partial class TestsInitializer
{

    [AssemblyInitialize]
    public static async Task Initialize(TestContext testContext)
    {
        await using var testServer = new AppTestServer();

        await testServer.Build(
        ).Start();

        await InitializeDatabase(testServer);

    }

    private static async Task InitializeDatabase(AppTestServer testServer)
    {
        if (testServer.WebApp.Environment.IsDevelopment())
        {
            await using var scope = testServer.WebApp.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }

}
