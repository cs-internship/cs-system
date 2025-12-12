using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaTaskService
{
    Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetUpdatesSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetRevisionsSyncItemsAsync(List<string> ids);

    // Split add/update operations into separate methods to allow batching and transactions
    Task AddCrystaTasksAsync(List<CrystaTask> tasks);
    Task UpdateCrystaTasksAsync(List<CrystaTask> tasks);

    Task AddCrystaTaskCommentsAsync(List<CrystaTaskComment> comments);
    Task UpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments);

    Task AddCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates);
    Task UpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates);

    Task AddCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions);
    Task UpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions);

    Task<int> MarkCrystaTasksAsDeletedAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskCommentsAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskUpdatesAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskRevisionsAsync(List<string> syncIds);
    Task<List<string>> GetAllWorkItemSyncIdsAsync(string project);
}
