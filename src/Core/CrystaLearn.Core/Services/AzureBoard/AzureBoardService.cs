using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Contracts;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Octokit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CrystaLearn.Core.Services.AzureBoard;
public partial class AzureBoardService : IAzureBoardService
{
    [AutoInject] IConfiguration Configuration { get; set; } = default!;

    public async Task<List<WorkItem>> GetWorkItemsRawQueryAsync(
        AzureBoardSyncConfig config, 
        string query, 
        string[]? fields = null,
        int? top = 200)
    {
        if (top > 200)
        {
            throw new InvalidOperationException("Top cannot be greater than 200");
        }

        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"), 
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));

        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        fields ??= [
            "System.Id",
            "System.Title", 
            "System.State",
            "System.Tags", 
            "System.WorkItemType", 
            "System.AssignedTo", 
            "System.CreatedDate", 
            "System.ChangedDate"
        ];

        var wiql = new Wiql { Query = query };

        var result = await witClient.QueryByWiqlAsync(wiql, top: top).ConfigureAwait(false);
        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        List<WorkItem> workItems = await witClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);

        return workItems;
    }

    public async IAsyncEnumerable<List<WorkItem>> EnumerateWorkItemsQueryAsync(AzureBoardSyncConfig config,
        string query,
        string[]? fields = null,
        int top = 19_999)
    {
        if (top > 19_999)
        {
            throw new InvalidOperationException("Top cannot be greater than 20,000");
        }

        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"),
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));

        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        // https://learn.microsoft.com/en-us/azure/devops/boards/work-items/guidance/work-item-field?view=azure-devops

        fields ??= [
            "System.Id",
            "System.Title",
            "System.Description",
            "System.State",
            "System.Tags",
            "System.WorkItemType",
            "System.AssignedTo",
            "System.CreatedDate",
            "System.ChangedDate",
            "System.AreaPath",
            "System.IterationPath",
            "System.Parent"
        ];

        var wiql = new Wiql { Query = query };

        var result = await witClient.QueryByWiqlAsync(wiql, top: top).ConfigureAwait(false);
        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        var workItemChunks = ids.Chunk(200);

        foreach (var chunk in workItemChunks)
        {
            List<WorkItem> workItems = await witClient.GetWorkItemsAsync(chunk, fields, result.AsOf).ConfigureAwait(false);
            yield return workItems;
        }
    }

    public async Task<List<WorkItem>> GetWorkItemsBatchAsync(AzureBoardSyncConfig config, string query, string[]? fields = null)
    {
        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"),
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));

        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        fields ??= [
            "System.Id",
            "System.Title",
            "System.State",
            "System.Tags",
            "System.WorkItemType",
            "System.AssignedTo",
            "System.CreatedDate",
            "System.ChangedDate"
        ];

        var wiql = new Wiql { Query = query };

        var result = await witClient.QueryByWiqlAsync(wiql, top: 100).ConfigureAwait(false);
        var ids = result.WorkItems.Select(item => item.Id).ToArray();
        
        List<WorkItem> workItems = await witClient.GetWorkItemsBatchAsync(new WorkItemBatchGetRequest
        {
            Ids = ids,
            AsOf = result.AsOf,
            Fields = fields
        });

        return workItems;
    }

    public async Task<List<WorkItem>> GetRevisionsAsync(AzureBoardSyncConfig config, int workItemId, int top = 200)
    {
        if (top > 200)
        {
            throw new InvalidOperationException("Top cannot be greater than 200");
        }

        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"), 
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));
        
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        var result = await witClient.GetRevisionsAsync(workItemId, top: top).ConfigureAwait(false);
        return result;
    }

    public async Task<List<WorkItemUpdate>> GetUpdatesAsync(AzureBoardSyncConfig config, int workItemId, int top = 200)
    {
        if (top > 200)
        {
            throw new InvalidOperationException("Top cannot be greater than 200");
        }

        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"), 
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));
        
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        var result = await witClient.GetUpdatesAsync(workItemId, top: top).ConfigureAwait(false);
        return result;
    }

    public async Task<List<WorkItemComment>> GetCommentsAsync(AzureBoardSyncConfig config, int workItemId, int top = 200)
    {
        if (top > 200)
        {
            throw new InvalidOperationException("Top cannot be greater than 200");
        }

        VssConnection connection = new VssConnection(
            new Uri($"https://dev.azure.com/{config.Organization}"), 
            new VssBasicCredential(string.Empty, config.PersonalAccessToken));
        
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        var result = await witClient.GetCommentsAsync(workItemId, top: top).ConfigureAwait(false);
        return result.Comments.ToList();
    }
}
