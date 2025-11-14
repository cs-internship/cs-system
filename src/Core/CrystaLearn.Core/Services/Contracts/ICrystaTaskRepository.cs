using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaTaskRepository
{
    Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetUpdatesSyncItemsAsync(List<string> ids);
    Task<List<SyncItem>> GetRevisionsSyncItemsAsync(List<string> ids);
    Task AddOrUpdateCrystaTasksAsync(List<CrystaTask> tasks);
    Task AddOrUpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments);
    Task AddOrUpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates);
    Task AddOrUpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions);
    Task<int> MarkCrystaTasksAsDeletedAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskCommentsAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskUpdatesAsync(List<string> syncIds);
    Task<int> DeleteCrystaTaskRevisionsAsync(List<string> syncIds);
    Task<List<string>> GetAllWorkItemSyncIdsAsync(string project);
}
