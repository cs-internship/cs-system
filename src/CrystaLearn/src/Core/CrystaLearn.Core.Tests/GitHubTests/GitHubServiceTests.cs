using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;

namespace CrystaLearn.Core.Tests.GitHubTests;
public class GitHubServiceTests : TestBase
{
    [Fact]
    public async Task GetFilesAsync_WhenCalled_ReturnsFiles()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IGitHubService>();
        var url = "https://github.com/cs-internship/cs-internship-spec/tree/master/processes/documents";

        // Act
        var files = await service.GetFilesAsync(url);

        // Assert
        Assert.True(files.Any());
    }
}
