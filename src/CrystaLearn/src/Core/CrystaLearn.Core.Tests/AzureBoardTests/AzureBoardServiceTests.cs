using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;

namespace CrystaLearn.Core.Tests.AzureBoardTests;
public class AzureBoardServiceTests : TestBase
{
    [Fact]
    public async Task GetWorkItemsAsync_WhenCalled_Returns()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IAzureBoardService>();
        var url = "https://github.com/cs-internship/cs-internship-spec/tree/master/processes";

        // Act
        await service.GetWorkItemsAsync();

        // Assert
    }
}
