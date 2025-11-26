using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;
public class CrystaTask : Entity
{
    public string? ProviderTaskId { get; set; } = default;
    public User? AssignedTo { get; set; }
    public User? CompletedBy { get; set; }
    public User? CreatedBy { get; set; }
    public string? AssignedToText { get; set; }
    public string? CompletedByText { get; set; }
    public string? CreatedByText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? DescriptionHtml { get; set; }
    public DateTimeOffset? TaskCreateDateTime { get; set; }
    public DateTimeOffset? TaskDoneDateTime { get; set; }
    public DateTimeOffset? TaskAssignDateTime { get; set; }
    public CrystaTaskStatus? Status { get; set; }
    public string? ProviderTaskUrl { get; set; }
    public SyncInfo WorkItemSyncInfo { get; set; } = new();
    public SyncInfo RevisionsSyncInfo { get; set; } = new();
    public SyncInfo UpdatesSyncInfo { get; set; } = new();
    public SyncInfo CommentsSyncInfo { get; set; }
    public CrystaProgram? CrystaProgram { get; set; }

    public Guid? ParentId { get; set; }

    [MaxLength(100)]
    public string? WorkItemType { get; set; }

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(200)]
    public string? Reason { get; set; }

    [MaxLength(500)]
    public string? AreaPath { get; set; }

    [MaxLength(500)]
    public string? IterationPath { get; set; }

    [MaxLength(255)]
    public string? CreatedByDisplayName { get; set; }

    [MaxLength(200)]
    public string? CreatedById { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }

    [MaxLength(255)]
    public string? ChangedBy { get; set; }

    [MaxLength(200)]
    public string? ChangedById { get; set; }

    public DateTimeOffset? ChangedDate { get; set; }

    [MaxLength(255)]
    public string? RevisedBy { get; set; }

    public string Revision { get; set; }

    public Guid? ProjectId { get; set; }

    [MaxLength(255)]
    public string? ProjectName { get; set; }

    public Guid? AreaId { get; set; }
    public Guid? IterationId { get; set; }

    [MaxLength(100)]
    public string? Severity { get; set; }

    public int? Priority { get; set; }
    public double? OriginalEstimate { get; set; }
    public double? RemainingWork { get; set; }
    public double? CompletedWork { get; set; }
    public double? StoryPoints { get; set; }

    [MaxLength(2000)]
    public string? Tags { get; set; }

    public int AttachmentsCount { get; set; }

    public string? Relations { get; set; }
    public string? Links { get; set; }

    public bool IsDeleted { get; set; }

    public int CommentCount { get; set; }

    [MaxLength(100)]
    public string? BoardColumn { get; set; }

    public bool BoardColumnDone { get; set; }

    public DateTimeOffset? StateChangeDate { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? ClosedDate { get; set; }
    public DateTimeOffset? ResolvedDate { get; set; }

    [MaxLength(200)]
    public string? ExternalId { get; set; }

    public string? CustomFields { get; set; }
    public string? RawJson { get; set; }
    public string? SystemFields { get; set; }

    public int? CreatedFromRevisionId { get; set; }

    public override string ToString()
    {
        return $"{Title} / {Description}";
    }
}
