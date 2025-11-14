using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services;

public class CrystaTaskRepositoryFake : ICrystaTaskRepository
{
    public async Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids)
    {
        return [];
    }

    public async Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids)
    {
        return [];
    }

    public async Task<List<SyncItem>> GetUpdatesSyncItemsAsync(List<string> ids)
    {
        return [];
    }

    public async Task<List<SyncItem>> GetRevisionsSyncItemsAsync(List<string> ids)
    {
        return [];
    }

    public async Task AddOrUpdateCrystaTasksAsync(List<CrystaTask> tasks)
    {
        // Fake implementation - does nothing
    }

    public async Task AddOrUpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        // Fake implementation - does nothing
    }

    public async Task AddOrUpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        // Fake implementation - does nothing
    }

    public async Task AddOrUpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        // Fake implementation - does nothing
    }
}
