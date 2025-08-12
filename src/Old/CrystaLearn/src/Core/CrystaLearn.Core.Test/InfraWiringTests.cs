using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;

namespace CrystaLearn.Core.Tests;

public class InfraWiringTests : TestBase
{
    [Fact]
    public async Task TestWiringTest()
    {
        var services = CreateServiceProvider();
        var repo = services.GetRequiredService<ICrystaProgramRepository>();
        var programs = await repo.GetCrystaProgramsAsync(CancellationToken.None);
        Assert.True(programs.Any());
    }
}
