using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.AzureBoard;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CrystaLearn.Core.Services.Contracts;
public interface IAzureBoardService
{
    Task<List<WorkItem>> GetWorkItemsRawQueryAsync(AzureBoardSyncConfig config, string query, string[]? fields = null, int? top = 200);
    Task<List<WorkItem>> GetWorkItemsBatchAsync(AzureBoardSyncConfig config, string query, string[]? fields = null);
    Task<List<WorkItem>> GetRevisionsAsync(int workItemId);
    Task<List<WorkItemUpdate>> GetUpdatesAsync(int workItemId);

    IAsyncEnumerable<List<WorkItem>> EnumerateWorkItemsQueryAsync(
        AzureBoardSyncConfig config,
        string query,
        string[]? fields = null,
        int top = 19_999);
}
