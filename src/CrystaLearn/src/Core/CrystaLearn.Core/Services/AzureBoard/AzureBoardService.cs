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

    public async Task<List<WorkItem>> GetWorkItemsRawQueryAsync(string query, string[] fields)
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];
        VssConnection connection = new VssConnection(new Uri("https://dev.azure.com/cs-internship"), new VssBasicCredential(string.Empty, pat));
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        //List<QueryHierarchyItem> queryHierarchyItems = await witClient.GetQueriesAsync("CS Internship Program", depth: 2);

        var wiql = new Wiql { Query = query };

        var result = await witClient.QueryByWiqlAsync(wiql, top: 100).ConfigureAwait(false);
        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        List<WorkItem> workItems = await witClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);

        return workItems;
    }

    public async Task<List<WorkItem>> GetWorkItemsBatchAsync(string query, string[] fields)
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];
        VssConnection connection = new VssConnection(new Uri("https://dev.azure.com/cs-internship"), new VssBasicCredential(string.Empty, pat));
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

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

    public async Task<List<WorkItem>> GetRevisionsAsync(int workItemId)
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];
        VssConnection connection = new VssConnection(new Uri("https://dev.azure.com/cs-internship"), new VssBasicCredential(string.Empty, pat));
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();

        var result = await witClient.GetRevisionsAsync(workItemId, top: 100).ConfigureAwait(false);
        return result;
    }

    public async Task<List<WorkItemUpdate>> GetUpdatesAsync(int workItemId)
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];
        VssConnection connection = new VssConnection(new Uri("https://dev.azure.com/cs-internship"), new VssBasicCredential(string.Empty, pat));
        using WorkItemTrackingHttpClient witClient = await connection.GetClientAsync<WorkItemTrackingHttpClient>();


        var result = await witClient.GetUpdatesAsync(workItemId, top: 100).ConfigureAwait(false);
        return result;
    }
}
