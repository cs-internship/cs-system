using CrystaLearn.Core.Data;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services;

public partial class CrystaTaskRepository : ICrystaTaskRepository
{
    [AutoInject] private AppDbContext DbContext { get; set; } = default!;

    public async Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return [];
        }

        var syncItems = await DbContext.CrystaTasks
            .Where(t => ids.Contains(t.WorkItemSyncInfo.SyncId ?? ""))
            .Select(t => new SyncItem
            {
                Id = t.Id,
                SyncInfo = t.WorkItemSyncInfo
            })
            .ToListAsync();

        return syncItems;
    }

    public async Task<List<SyncItem>> GetCommentsSyncItemsAsync(List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return [];
        }

        var syncItems = await DbContext.CrystaTaskComments
            .Where(c => c.SyncInfo != null && ids.Contains(c.SyncInfo.SyncId ?? ""))
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
            return [];
        }

        var syncItems = await DbContext.CrystaTaskUpdates
            .Where(u => u.SyncInfo != null && ids.Contains(u.SyncInfo.SyncId ?? ""))
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
            return [];
        }

        var syncItems = await DbContext.CrystaTaskRevisions
            .Select(r => new SyncItem
            {
                Id = r.Id,
                SyncInfo = new SyncInfo
                {
                    SyncId = $"{r.AzureWorkItemId}-{r.RevisionNumber}",
                    ContentHash = r.SnapshotJson != null ? r.SnapshotJson : ""
                }
            })
            .Where(s => ids.Contains(s.SyncInfo.SyncId ?? ""))
            .ToListAsync();

        return syncItems;
    }

    public async Task AddOrUpdateCrystaTasksAsync(List<CrystaTask> tasks)
    {
        if (tasks == null || tasks.Count == 0)
        {
            return;
        }

        foreach (var task in tasks)
        {
            var existingTask = await DbContext.CrystaTasks
                .FirstOrDefaultAsync(t => t.WorkItemSyncInfo.SyncId == task.WorkItemSyncInfo.SyncId);

            if (existingTask == null)
            {
                // Add new task
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTasks.Add(task);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
            else
            {
                // Update existing task
                existingTask.ProviderTaskId = task.ProviderTaskId;
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DescriptionHtml = task.DescriptionHtml;
                existingTask.Status = task.Status;
                existingTask.TaskCreateDateTime = task.TaskCreateDateTime;
                existingTask.TaskDoneDateTime = task.TaskDoneDateTime;
                existingTask.TaskAssignDateTime = task.TaskAssignDateTime;
                existingTask.ProviderTaskUrl = task.ProviderTaskUrl;
                existingTask.AssignedToText = task.AssignedToText;
                existingTask.CompletedByText = task.CompletedByText;
                existingTask.CreatedByText = task.CreatedByText;
                existingTask.WorkItemSyncInfo = task.WorkItemSyncInfo;
                existingTask.RevisionsSyncInfo = task.RevisionsSyncInfo;
                existingTask.UpdatesSyncInfo = task.UpdatesSyncInfo;
                existingTask.CommentsSyncInfo = task.CommentsSyncInfo;
                
                // Update Azure DevOps specific fields
                existingTask.WorkItemType = task.WorkItemType;
                existingTask.State = task.State;
                existingTask.Reason = task.Reason;
                existingTask.AreaPath = task.AreaPath;
                existingTask.IterationPath = task.IterationPath;
                existingTask.Revision = task.Revision;
                existingTask.Priority = task.Priority;
                existingTask.Tags = task.Tags;
                existingTask.RawJson = task.RawJson;
                
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTasks.Update(existingTask);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
        }

        await DbContext.SaveChangesAsync();
    }

    public async Task AddOrUpdateCrystaTaskCommentsAsync(List<CrystaTaskComment> comments)
    {
        if (comments == null || comments.Count == 0)
        {
            return;
        }

        foreach (var comment in comments)
        {
            var existingComment = await DbContext.CrystaTaskComments
                .FirstOrDefaultAsync(c => c.SyncInfo != null && c.SyncInfo.SyncId == comment.SyncInfo!.SyncId);

            if (existingComment == null)
            {
                // Add new comment
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskComments.Add(comment);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
            else
            {
                // Update existing comment
                existingComment.Text = comment.Text;
                existingComment.FormattedText = comment.FormattedText;
                existingComment.CreatedBy = comment.CreatedBy;
                existingComment.CreatedById = comment.CreatedById;
                existingComment.CreatedDate = comment.CreatedDate;
                existingComment.EditedBy = comment.EditedBy;
                existingComment.EditedById = comment.EditedById;
                existingComment.EditedDate = comment.EditedDate;
                existingComment.IsDeleted = comment.IsDeleted;
                existingComment.Revision = comment.Revision;
                existingComment.RawJson = comment.RawJson;
                existingComment.SyncInfo = comment.SyncInfo;
                
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskComments.Update(existingComment);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
        }

        await DbContext.SaveChangesAsync();
    }

    public async Task AddOrUpdateCrystaTaskUpdatesAsync(List<CrystaTaskUpdate> updates)
    {
        if (updates == null || updates.Count == 0)
        {
            return;
        }

        foreach (var update in updates)
        {
            var existingUpdate = await DbContext.CrystaTaskUpdates
                .FirstOrDefaultAsync(u => u.SyncInfo != null && u.SyncInfo.SyncId == update.SyncInfo!.SyncId);

            if (existingUpdate == null)
            {
                // Add new update
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskUpdates.Add(update);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
            else
            {
                // Update existing update
                existingUpdate.Revision = update.Revision;
                existingUpdate.ChangedBy = update.ChangedBy;
                existingUpdate.ChangedById = update.ChangedById;
                existingUpdate.ChangedDate = update.ChangedDate;
                existingUpdate.FieldName = update.FieldName;
                existingUpdate.FieldDisplayName = update.FieldDisplayName;
                existingUpdate.OldValue = update.OldValue;
                existingUpdate.NewValue = update.NewValue;
                existingUpdate.Operation = update.Operation;
                existingUpdate.CommentText = update.CommentText;
                existingUpdate.RawJson = update.RawJson;
                existingUpdate.SyncInfo = update.SyncInfo;
                
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskUpdates.Update(existingUpdate);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
        }

        await DbContext.SaveChangesAsync();
    }

    public async Task AddOrUpdateCrystaTaskRevisionsAsync(List<CrystaTaskRevision> revisions)
    {
        if (revisions == null || revisions.Count == 0)
        {
            return;
        }

        foreach (var revision in revisions)
        {
            var existingRevision = await DbContext.CrystaTaskRevisions
                .FirstOrDefaultAsync(r => r.AzureWorkItemId == revision.AzureWorkItemId && 
                                         r.RevisionNumber == revision.RevisionNumber);

            if (existingRevision == null)
            {
                // Add new revision
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskRevisions.Add(revision);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
            else
            {
                // Update existing revision
                existingRevision.SnapshotJson = revision.SnapshotJson;
                existingRevision.Title = revision.Title;
                existingRevision.Description = revision.Description;
                existingRevision.State = revision.State;
                existingRevision.ChangedBy = revision.ChangedBy;
                existingRevision.ChangedDate = revision.ChangedDate;
                existingRevision.CreatedDate = revision.CreatedDate;
                existingRevision.ProjectId = revision.ProjectId;
                existingRevision.ProjectName = revision.ProjectName;
                existingRevision.FieldsSnapshot = revision.FieldsSnapshot;
                existingRevision.RelationsSnapshot = revision.RelationsSnapshot;
                existingRevision.AttachmentsSnapshot = revision.AttachmentsSnapshot;
                existingRevision.CommentsSnapshot = revision.CommentsSnapshot;
                existingRevision.ChangeSummary = revision.ChangeSummary;
                existingRevision.ChangeDetails = revision.ChangeDetails;
                existingRevision.RawJson = revision.RawJson;
                
#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
                DbContext.CrystaTaskRevisions.Update(existingRevision);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
            }
        }

        await DbContext.SaveChangesAsync();
    }
}
