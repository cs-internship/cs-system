﻿using System;
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
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? DescriptionHtml { get; set; }
    public DateTimeOffset? TaskCreateDateTime { get; set; }
    public DateTimeOffset? TaskDoneDateTime { get; set; }
    public DateTimeOffset? TaskAssignDateTime { get; set; }
    public CrystaTaskStatus? Status { get; set; }
    public string? ProviderTaskUrl { get; set; }
    public SyncInfo? WorkItemSyncInfo { get; set; }
    public SyncInfo? RevisionsSyncInfo { get; set; }
    public SyncInfo? UpdatesSyncInfo { get; set; }
    public SyncInfo? CommentsSyncInfo { get; set; }
    public CrystaProgram? CrystaProgram { get; set; }
}
