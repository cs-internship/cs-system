using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services;

public class CrystaTaskServiceFake : ICrystaTaskService
{
    // In-memory stores for the fake implementation
    private readonly List<CrystaTask> _tasks = new();
    private readonly List<CrystaTaskComment> _comments = new();
    private readonly List<CrystaTaskRevision> _revisions = new();
    private readonly List<CrystaTaskUpdate> _updates = new();

    private static Guid EnsureId(Guid id) => id == Guid.Empty ? Guid.NewGuid() : id;

    public Task AddCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        if (comments == null) return Task.CompletedTask;
        foreach (var c in comments)
        {
            c.Id = EnsureId(c.Id);
            _comments.Add(c);
        }
        return Task.CompletedTask;
    }

    public Task AddCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        if (revisions == null) return Task.CompletedTask;
        foreach (var r in revisions)
        {
            r.Id = EnsureId(r.Id);
            _revisions.Add(r);
        }
        return Task.CompletedTask;
    }

    public Task AddCrystaTasksAsync(List<CrystaTask> tasks)
    {
        if (tasks == null) return Task.CompletedTask;
        foreach (var t in tasks)
        {
            t.Id = EnsureId(t.Id);
            _tasks.Add(t);
        }
        return Task.CompletedTask;
    }

    public Task AddCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        if (updates == null) return Task.CompletedTask;
        foreach (var u in updates)
        {
            u.Id = EnsureId(u.Id);
            _updates.Add(u);
        }
        return Task.CompletedTask;
    }

    public Task<int> DeleteCrystaTaskCommentsAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0) return Task.FromResult(0);
        int marked = 0;
        foreach (var c in _comments)
        {
            if (
                (c.Revision != null && syncIds.Contains(c.Revision)) ||
                (c.ProviderTaskId != null && syncIds.Contains(c.ProviderTaskId)) ||
                (c.Id != Guid.Empty && syncIds.Contains(c.Id.ToString()))
            )
            {
                if (c.IsDeleted != true)
                {
                    c.IsDeleted = true;
                    c.SyncStatus = CrystaSyncStatus.Deleted;
                    marked++;
                }
            }
        }
        return Task.FromResult(marked);
    }

    public Task<int> DeleteCrystaTaskRevisionsAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0) return Task.FromResult(0);
        var removed = _revisions.RemoveAll(r =>
            (r.Revision != null && syncIds.Contains(r.Revision)) ||
            (r.ProviderTaskId != null && syncIds.Contains(r.ProviderTaskId)) ||
            (r.Id != Guid.Empty && syncIds.Contains(r.Id.ToString())));
        return Task.FromResult(removed);
    }

    public Task<int> DeleteCrystaTaskUpdatesAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0) return Task.FromResult(0);
        var removed = _updates.RemoveAll(u =>
            u.SyncInfo != null && !string.IsNullOrEmpty(u.SyncInfo.SyncId) && syncIds.Contains(u.SyncInfo.SyncId));
        return Task.FromResult(removed);
    }

    public Task<List<string>> GetAllWorkItemSyncIdsAsync(string project)
    {
        // Get all non-deleted tasks sync IDs matching the project
        var syncIds = _tasks
            .Where(t => !t.IsDeleted && t.ProjectName == project && !string.IsNullOrEmpty(t.WorkItemSyncInfo.SyncId))
            .Select(t => t.WorkItemSyncInfo.SyncId ?? "")
            .Where(id => !string.IsNullOrEmpty(id))
            .ToList();

        return Task.FromResult(syncIds);
    }

    public Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0) return Task.FromResult(new List<SyncItem>());
        var found = _comments
            .Where(c => c.SyncInfo != null && ids.Contains(c.SyncInfo.SyncId ?? ""))
            .Select(c => new SyncItem { Id = c.Id, SyncInfo = c.SyncInfo })
            .ToList();
        return Task.FromResult(found);
    }

    public Task<List<SyncItem>> GetRevisionsSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0) return Task.FromResult(new List<SyncItem>());
        var found = _revisions
            .Where(r => ids.Contains(r.WorkItemSyncInfo.SyncId ?? ""))
            .Select(r => new SyncItem { Id = r.Id, SyncInfo = r.WorkItemSyncInfo })
            .ToList();
        return Task.FromResult(found);
    }

    public Task<List<SyncItem>> GetUpdatesSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0) return Task.FromResult(new List<SyncItem>());
        var found = _updates
            .Where(u => u.SyncInfo != null && ids.Contains(u.SyncInfo.SyncId ?? ""))
            .Select(u => new SyncItem { Id = u.Id, SyncInfo = u.SyncInfo })
            .ToList();
        return Task.FromResult(found);
    }

    public Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0) return Task.FromResult(new List<SyncItem>());
        var found = _tasks
            .Where(t => ids.Contains(t.WorkItemSyncInfo.SyncId ?? ""))
            .Select(t => new SyncItem { Id = t.Id, SyncInfo = t.WorkItemSyncInfo })
            .ToList();
        return Task.FromResult(found);
    }

    public Task<int> MarkCrystaTasksAsDeletedAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0) return Task.FromResult(0);
        var matched = _tasks.Where(t => (t.ProviderTaskId != null && syncIds.Contains(t.ProviderTaskId)) || (t.Id != Guid.Empty && syncIds.Contains(t.Id.ToString()))).ToList();
        foreach (var t in matched)
        {
            t.IsDeleted = true;
        }
        return Task.FromResult(matched.Count);
    }

    public Task UpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        if (comments == null) return Task.CompletedTask;
        foreach (var c in comments)
        {
            var existing = _comments.FirstOrDefault(x => x.Id == c.Id || (!string.IsNullOrEmpty(x.Revision) && x.Revision == c.Revision));
            if (existing != null)
            {
                // naive replace of reference properties
                var idx = _comments.IndexOf(existing);
                c.Id = existing.Id;
                _comments[idx] = c;
            }
            else
            {
                c.Id = EnsureId(c.Id);
                _comments.Add(c);
            }
        }
        return Task.CompletedTask;
    }

    public Task UpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        if (revisions == null) return Task.CompletedTask;
        foreach (var r in revisions)
        {
            var existing = _revisions.FirstOrDefault(x =>
                x.Id == r.Id ||
                (
                    !string.IsNullOrEmpty(x.ProviderTaskId) &&
                    !string.IsNullOrEmpty(r.ProviderTaskId) &&
                    x.ProviderTaskId == r.ProviderTaskId &&
                    !string.IsNullOrEmpty(x.Revision) &&
                    !string.IsNullOrEmpty(r.Revision) &&
                    x.Revision == r.Revision
                )
            );
            if (existing != null)
            {
                var idx = _revisions.IndexOf(existing);
                r.Id = existing.Id;
                _revisions[idx] = r;
            }
            else
            {
                r.Id = EnsureId(r.Id);
                _revisions.Add(r);
            }
        }
        return Task.CompletedTask;
    }

    public Task UpdateCrystaTasksAsync(List<CrystaTask> tasks)
    {
        if (tasks == null) return Task.CompletedTask;
        foreach (var t in tasks)
        {
            var existing = _tasks.FirstOrDefault(x => x.Id == t.Id || (!string.IsNullOrEmpty(x.ProviderTaskId) && x.ProviderTaskId == t.ProviderTaskId));
            if (existing != null)
            {
                var idx = _tasks.IndexOf(existing);
                t.Id = existing.Id;
                _tasks[idx] = t;
            }
            else
            {
                t.Id = EnsureId(t.Id);
                _tasks.Add(t);
            }
        }
        return Task.CompletedTask;
    }

    public Task UpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        if (updates == null) return Task.CompletedTask;
        foreach (var u in updates)
        {
            var existing = _updates.FirstOrDefault(x => x.SyncInfo?.SyncId == u.SyncInfo?.SyncId);
            if (existing != null)
            {
                var idx = _updates.IndexOf(existing);
                u.Id = existing.Id;
                _updates[idx] = u;
            }
            else
            {
                u.Id = EnsureId(u.Id);
                _updates.Add(u);
            }
        }
        return Task.CompletedTask;
    }
}
