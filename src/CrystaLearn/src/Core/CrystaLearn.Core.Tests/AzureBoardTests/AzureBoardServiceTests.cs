using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Tests.Infra;
using Octokit;

namespace CrystaLearn.Core.Tests.AzureBoardTests;
public class AzureBoardServiceTests : TestBase
{
    [Fact]
    public async Task GetWorkItemsAsync_WhenCalled_Returns()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IAzureBoardService>();

        var project = "CS Internship Program";
        var query =
            $"""
            Select 
                [Id] 
            From 
                WorkItems 
            Where 
                [Work Item Type] = 'Task' 
                And [System.TeamProject] = '{project}' 
                And [System.State] <> 'Closed' 
            Order By 
                [State] Asc, 
                [Changed Date] Desc
            """;

        var fields = new string[] { "System.Id", "System.Title", "System.State", "System.Tags", "System.WorkItemType", "System.AssignedTo", "System.CreatedDate", "System.ChangedDate" };

        // Act
        var list = await service.GetWorkItemsRawQueryAsync(query, fields);

        // Assert
    }

    [Fact]
    public async Task GetWorkItemsBatchAsync_WhenCalled_Returns()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IAzureBoardService>();

        var project = "CS Internship Program";
        var query =
            $"""
             Select 
                 [Id] 
             From 
                 WorkItems 
             Where 
                 [Work Item Type] = 'Task' 
                 And [System.TeamProject] = '{project}' 
                 And [System.State] <> 'Closed' 
             Order By 
                 [State] Asc, 
                 [Changed Date] Desc
             """;

        var fields = new string[] { "System.Id", "System.Title", "System.State", "System.Tags", "System.WorkItemType", "System.AssignedTo", "System.CreatedDate", "System.ChangedDate" };

        // Act
        var list = await service.GetWorkItemsBatchAsync(query, fields);

        // Assert
    }

    [Fact]
    public async Task GetRevisionsAsync_WhenCalled_Returns()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IAzureBoardService>();


        // Act
        var fields = new string[] { "System.Id", "System.Title", "System.State", "System.Tags", "System.WorkItemType", "System.AssignedTo", "System.CreatedDate", "System.ChangedDate" };
        var list = await service.GetRevisionsAsync(10455);

        // Assert
    }

    [Fact]
    public async Task GetUpdatesAsync_WhenCalled_Returns()
    {
        // Arrange
        var services = CreateServiceProvider();
        var service = services.GetRequiredService<IAzureBoardService>();


        // Act
        var fields = new string[] { "System.Id", "System.Title", "System.State", "System.Tags", "System.WorkItemType", "System.AssignedTo", "System.CreatedDate", "System.ChangedDate" };
        var list = await service.GetUpdatesAsync(10455);

        // Assert
    }
}
