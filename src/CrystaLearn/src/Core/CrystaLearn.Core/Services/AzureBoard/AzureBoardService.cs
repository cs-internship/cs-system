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

namespace CrystaLearn.Core.Services.AzureBoard;
public partial class AzureBoardService : IAzureBoardService
{
    [AutoInject] IConfiguration Configuration { get; set; } = default!;

    public async Task GetWorkItemsAsync()
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];
        VssConnection connection = new VssConnection(new Uri("https://dev.azure.com/cs-internship"), new VssBasicCredential(string.Empty, pat));
        using WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
        
        //List<QueryHierarchyItem> queryHierarchyItems = await witClient.GetQueriesAsync("CS Internship Program", depth: 2);

        var project = "CS Internship Program";

        var wiql = new Wiql()
        {
            Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [Work Item Type] = 'Task' " +
                    "And [System.TeamProject] = '" + project + "' " +
                    "And [System.State] <> 'Closed' " +
                    "Order By [State] Asc, [Changed Date] Desc"
            
        };

        List<WorkItem> workItems;

        try
        {
            var result = await witClient.QueryByWiqlAsync(wiql,top:100).ConfigureAwait(false);
            var ids = result.WorkItems.Select(item => item.Id).ToArray();

            if (ids.Length == 0)
            {
                workItems = [];
            }

            var fields = new[] { "System.Id", "System.Title", "System.State" };
            workItems = await witClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error querying work items: " + ex.Message);
            workItems = [];
        }
    }
}
