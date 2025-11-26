using CrystaLearn.Core.Data;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.Sync;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace CrystaLearn.Core.Services;

public partial class CrystaTaskRepository : ICrystaTaskRepository
{
    [AutoInject] private AppDbContext DbContext { get; set; } = default!;

    public async Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return new List<SyncItem>();
        }

        var syncItems = await DbContext.CrystaTasks
            .Where(t => ids.Contains(t.WorkItemSyncInfo.SyncId ?? ""))
            .Select(t => new SyncItem
            {
                Id = t.Id,
                SyncInfo = t.WorkItemSyncInfo
            }).AsNoTracking()
            .ToListAsync();

        return syncItems;
    }

    public async Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return new List<SyncItem>();
        }

        var syncItems = await DbContext.CrystaTaskComments
            .Where(c => c.SyncInfo != null && ids.Contains(c.SyncInfo.SyncId ?? ""))
            .AsNoTracking()
            .Select(c => new SyncItem
            {
                Id = c.Id,
                SyncInfo = c.SyncInfo ?? new SyncInfo()
            })
            .ToListAsync();

        return syncItems;
    }

    public async Task<List<SyncItem>> GetUpdatesSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return new List<SyncItem>();
        }

        var syncItems = await DbContext.CrystaTaskUpdates
            .Where(u => u.SyncInfo != null && ids.Contains(u.SyncInfo.SyncId ?? ""))
            .AsNoTracking()
            .Select(u => new SyncItem
            {
                Id = u.Id,
                SyncInfo = u.SyncInfo ?? new SyncInfo()
            })
            .ToListAsync();

        return syncItems;
    }

    public async Task<List<SyncItem>> GetRevisionsSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return new List<SyncItem>();
        }

        var syncItems = await DbContext.CrystaTaskRevisions
            .Select(r => new SyncItem
            {
                Id = r.Id,
                SyncInfo = new SyncInfo
                {
                    SyncId = $"{r.ProviderTaskId}-{r.Revision}",
                    ContentHash = r.RawJson != null ? r.RawJson : ""
                }
            })
            .Where(s => ids.Contains(s.SyncInfo.SyncId ?? ""))
            .ToListAsync();

        return syncItems;
    }

    private const int BatchSize = 1000;

    public async Task AddCrystaTasksAsync(List<CrystaTask> tasks)
    {
        if (tasks == null || tasks.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            foreach (var chunk in tasks.Chunk(BatchSize))
            {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTasks.AddRange(chunk);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
                await DbContext.SaveChangesAsync();
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateCrystaTasksAsync(List<CrystaTask> tasks)
    {
        if (tasks == null || tasks.Count == 0)
            return;

        // Gather sync ids from incoming items
        var syncIds = tasks.Select(t => t.WorkItemSyncInfo?.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        if (syncIds.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            // Fetch only Id and SyncId mapping (lightweight)
            var existingMap = await DbContext.CrystaTasks
                .Where(t => syncIds.Contains(t.WorkItemSyncInfo.SyncId ?? ""))
                .Select(t => new { t.Id, SyncId = t.WorkItemSyncInfo.SyncId })
                .ToDictionaryAsync(x => x.SyncId ?? string.Empty, x => x.Id);

            // Set primary keys on incoming entities so BulkUpdate uses PK matching
            var toUpdate = new List<CrystaTask>();
            foreach (var task in tasks)
            {
                var key = task.WorkItemSyncInfo?.SyncId ?? string.Empty;
                if (existingMap.TryGetValue(key, out var id))
                {
                    task.Id = id; // ensure PK present for BulkUpdate
                    toUpdate.Add(task);
                }
            }

            if (toUpdate.Count == 0)
            {
                await tx.CommitAsync();
                return;
            }

            // Bulk update in chunks
            foreach (var chunk in toUpdate.Chunk(BatchSize))
            {
                await DbContext.BulkUpdateAsync(chunk);
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task AddCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        if (comments == null || comments.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            foreach (var chunk in comments.Chunk(BatchSize))
            {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskComments.AddRange(chunk);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
                await DbContext.SaveChangesAsync();
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        if (comments == null || comments.Count == 0)
            return;

        var syncIds = comments.Select(c => c.SyncInfo?.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        if (syncIds.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            var existingMap = await DbContext.CrystaTaskComments
                .Where(c => c.SyncInfo != null && syncIds.Contains(c.SyncInfo.SyncId ?? ""))
                .Select(c => new { c.Id, SyncId = c.SyncInfo!.SyncId })
                .ToDictionaryAsync(x => x.SyncId ?? string.Empty, x => x.Id);

            var toUpdate = new List<CrystaTaskComment>();
            foreach (var comment in comments)
            {
                var key = comment.SyncInfo?.SyncId ?? string.Empty;
                if (existingMap.TryGetValue(key, out var id))
                {
                    comment.Id = id;
                    toUpdate.Add(comment);
                }
            }

            if (toUpdate.Count > 0)
            {
                foreach (var chunk in toUpdate.Chunk(BatchSize))
                {
                    await DbContext.BulkUpdateAsync(chunk);
                }
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task AddCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        if (updates == null || updates.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            foreach (var chunk in updates.Chunk(BatchSize))
            {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskUpdates.AddRange(chunk);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
                await DbContext.SaveChangesAsync();
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        if (updates == null || updates.Count == 0)
            return;

        var syncIds = updates.Select(u => u.SyncInfo?.SyncId ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        if (syncIds.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            var existingMap = await DbContext.CrystaTaskUpdates
                .Where(u => u.SyncInfo != null && syncIds.Contains(u.SyncInfo.SyncId ?? ""))
                .Select(u => new { u.Id, SyncId = u.SyncInfo!.SyncId })
                .ToDictionaryAsync(x => x.SyncId ?? string.Empty, x => x.Id);

            var toUpdate = new List<CrystaTaskUpdate>();
            foreach (var update in updates)
            {
                var key = update.SyncInfo?.SyncId ?? string.Empty;
                if (existingMap.TryGetValue(key, out var id))
                {
                    update.Id = id;
                    toUpdate.Add(update);
                }
            }

            if (toUpdate.Count > 0)
            {
                foreach (var chunk in toUpdate.Chunk(BatchSize))
                {
                    await DbContext.BulkUpdateAsync(chunk);
                }
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task AddCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        if (revisions == null || revisions.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            foreach (var chunk in revisions.Chunk(BatchSize))
            {
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskRevisions.AddRange(chunk);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
                await DbContext.SaveChangesAsync();
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        if (revisions == null || revisions.Count == 0)
            return;

        // Identify revisions by ProviderTaskId + Revision
        var keys = revisions.Select(r => (ProviderId: r.ProviderTaskId ?? string.Empty, Revision: r.Revision ?? string.Empty))
            .Where(k => !string.IsNullOrEmpty(k.ProviderId) && !string.IsNullOrEmpty(k.Revision))
            .ToList();

        if (keys.Count == 0)
            return;

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            var providerIds = keys.Select(k => k.ProviderId).Distinct().ToList();

            var existing = await DbContext.CrystaTaskRevisions
                .Where(r => providerIds.Contains(r.ProviderTaskId ?? string.Empty))
                .Select(r => new { r.Id, ProviderTaskId = r.ProviderTaskId ?? string.Empty, Revision = r.Revision ?? string.Empty })
                .ToListAsync();

            var map = existing.ToDictionary(e => $"{e.ProviderTaskId}-{e.Revision}", e => e.Id);

            var toUpdate = new List<CrystaTaskRevision>();

            foreach (var revision in revisions)
            {
                var key = $"{revision.ProviderTaskId}-{revision.Revision}";
                if (map.TryGetValue(key, out var id))
                {
                    revision.Id = id;
                    toUpdate.Add(revision);
                }
            }

            if (toUpdate.Count > 0)
            {
                foreach (var chunk in toUpdate.Chunk(BatchSize))
                {
                    await DbContext.BulkUpdateAsync(chunk);
                }
            }

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<int> MarkCrystaTasksAsDeletedAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0)
        {
            return 0;
        }

        var tasksToDelete = await DbContext.CrystaTasks
            .Where(t => syncIds.Contains(t.WorkItemSyncInfo.SyncId ?? ""))
            .ToListAsync();

        foreach (var task in tasksToDelete)
        {
            task.IsDeleted = true;
            task.WorkItemSyncInfo.SyncStatus = SyncStatus.Deleted;
            task.WorkItemSyncInfo.LastSyncDateTime = DateTimeOffset.Now;
        }

        await DbContext.SaveChangesAsync();
        return tasksToDelete.Count;
    }

    public async Task<int> DeleteCrystaTaskCommentsAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0)
        {
            return 0;
        }

        var commentsToDelete = await DbContext.CrystaTaskComments
            .Where(c => c.SyncInfo != null && syncIds.Contains(c.SyncInfo.SyncId ?? ""))
            .ToListAsync();

        foreach (var comment in commentsToDelete)
        {
            comment.IsDeleted = true;
            if (comment.SyncInfo != null)
            {
                comment.SyncInfo.SyncStatus = SyncStatus.Deleted;
                comment.SyncInfo.LastSyncDateTime = DateTimeOffset.Now;
            }
        }

        await DbContext.SaveChangesAsync();
        return commentsToDelete.Count;
    }

    public async Task<int> DeleteCrystaTaskUpdatesAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0)
        {
            return 0;
        }

        var updatesToDelete = await DbContext.CrystaTaskUpdates
            .Where(u => u.SyncInfo != null && syncIds.Contains(u.SyncInfo.SyncId ?? ""))
            .ToListAsync();

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
        DbContext.CrystaTaskUpdates.RemoveRange(updatesToDelete);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer

        await DbContext.SaveChangesAsync();
        return updatesToDelete.Count;
    }

    public async Task<int> DeleteCrystaTaskRevisionsAsync(List<string> syncIds)
    {
        if (syncIds == null || syncIds.Count == 0)
        {
            return 0;
        }

        var revisionsToDelete = await DbContext.CrystaTaskRevisions
            .Where(r => syncIds.Contains($"{r.ProviderTaskId}-{r.Revision}"))
            .ToListAsync();

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
        DbContext.CrystaTaskRevisions.RemoveRange(revisionsToDelete);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer

        await DbContext.SaveChangesAsync();
        return revisionsToDelete.Count;
    }

    public async Task<List<string>> GetAllWorkItemSyncIdsAsync(string project)
    {
        // Get all non-deleted tasks sync IDs
        var syncIds = await DbContext.CrystaTasks
            .Where(t => !t.IsDeleted)
            .Select(t => t.WorkItemSyncInfo.SyncId ?? "")
            .Where(id => !string.IsNullOrEmpty(id))
            .ToListAsync();

        return syncIds;
    }
}
