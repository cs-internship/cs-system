using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace CrystaLearn.Core.Services.Contracts;
public interface IAzureBoardService
{
    Task<List<WorkItem>> GetWorkItemsRawQueryAsync(string query, string[] fields);
    Task<List<WorkItem>> GetWorkItemsBatchAsync(string query, string[] fields);
    Task<List<WorkItem>> GetRevisionsAsync(int workItemId);
    Task<List<WorkItemUpdate>> GetUpdatesAsync(int workItemId);
}
