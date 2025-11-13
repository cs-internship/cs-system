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
    // Azure DevOps Work Item ID
    public int ProviderTaskId { get; set; }

    // Parent task reference
    public Guid? ParentId { get; set; }

    // Work item type (Task, Bug, User Story, etc.)
    [MaxLength(100)]
    public string? WorkItemType { get; set; }

    // Title
    [MaxLength(500)]
    public string? Title { get; set; }

    // Description / HTML content
    public string? Description { get; set; }

    // State (Active, Closed, etc.)
    [MaxLength(100)]
    public string? State { get; set; }

    // Reason for current state
    [MaxLength(200)]
    public string? Reason { get; set; }

    // Area path
    [MaxLength(500)]
    public string? AreaPath { get; set; }

    // Iteration path
    [MaxLength(500)]
    public string? IterationPath { get; set; }

    // Creator display name/email
    [MaxLength(255)]
    public string? CreatedBy { get; set; }

    // Creator identity id/descriptor
    [MaxLength(200)]
    public string? CreatedById { get; set; }

    // Creation timestamp
    public DateTimeOffset CreatedDate { get; set; }

    // Last changed by display name/email
    [MaxLength(255)]
    public string? ChangedBy { get; set; }

    // Last changer identity id/descriptor
    [MaxLength(200)]
    public string? ChangedById { get; set; }

    // Last changed timestamp
    public DateTimeOffset ChangedDate { get; set; }

    // Revised by (for snapshots)
    [MaxLength(255)]
    public string? RevisedBy { get; set; }

    // Revision number
    public int Revision { get; set; }

    // Project identifiers
    public Guid ProjectId { get; set; }

    [MaxLength(255)]
    public string? ProjectName { get; set; }

    // Area and Iteration IDs
    public Guid? AreaId { get; set; }
    public Guid? IterationId { get; set; }

    // Work tracking fields
    [MaxLength(100)]
    public string? Severity { get; set; }

    public int? Priority { get; set; }
    public double? OriginalEstimate { get; set; }
    public double? RemainingWork { get; set; }
    public double? CompletedWork { get; set; }
    public double? StoryPoints { get; set; }

    // Tags
    [MaxLength(2000)]
    public string? Tags { get; set; }

    // Attachments and relations
    public int AttachmentsCount { get; set; }

    public string? Relations { get; set; }
    public string? Links { get; set; }

    // Provider URL
    [MaxLength(1000)]
    public string? ProviderTaskUrl { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; }

    // Comment count
    public int CommentCount { get; set; }

    // Board columns
    [MaxLength(100)]
    public string? BoardColumn { get; set; }

    public bool BoardColumnDone { get; set; }

    // Date fields
    public DateTimeOffset? StateChangeDate { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? ClosedDate { get; set; }
    public DateTimeOffset? ResolvedDate { get; set; }

    // External mapping
    [MaxLength(200)]
    public string? ExternalId { get; set; }

    // Raw data storage
    public string? CustomFields { get; set; }
    public string? RawJson { get; set; }
    public string? SystemFields { get; set; }

    // Revision link
    public int? CreatedFromRevisionId { get; set; }

    // Sync information
    public SyncInfo WorkItemSyncInfo { get; set; } = new();
    public SyncInfo RevisionsSyncInfo { get; set; } = new();
    public SyncInfo UpdatesSyncInfo { get; set; } = new();
    public SyncInfo CommentsSyncInfo { get; set; } = new();

    // Legacy fields for backward compatibility (kept)
    public User? AssignedTo { get; set; }
    public User? CompletedBy { get; set; }
    public User? CreatedByUser { get; set; }
    
    [MaxLength(255)]
    public string? AssignedToText { get; set; }
    
    [MaxLength(255)]
    public string? CompletedByText { get; set; }
    
    [MaxLength(255)]
    public string? CreatedByText { get; set; }
    
    [MaxLength(2000)]
    public string? DescriptionHtml { get; set; }
    
    public DateTimeOffset? TaskCreateDateTime { get; set; }
    public DateTimeOffset? TaskDoneDateTime { get; set; }
    public DateTimeOffset? TaskAssignDateTime { get; set; }
    public CrystaTaskStatus? Status { get; set; }

    // Program reference
    public CrystaProgram? CrystaProgram { get; set; }

    public override string ToString()
    {
        return $"{Title} / {Description}";
    }
}
