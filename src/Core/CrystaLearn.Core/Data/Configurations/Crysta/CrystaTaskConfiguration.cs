using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskConfiguration : IEntityTypeConfiguration<CrystaTask>
{
    public void Configure(EntityTypeBuilder<CrystaTask> builder)
    {
        builder.HasIndex(t => t.ProviderTaskId);
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.TaskCreateDateTime);
        builder.HasIndex(t => t.Status);
        builder.OwnsOne(u => u.WorkItemSyncInfo, sync =>
        {
            sync.HasIndex(s => s.SyncId).IsUnique();
        });

        builder.OwnsOne(t => t.RevisionsSyncInfo);
        builder.OwnsOne(t => t.UpdatesSyncInfo);
        builder.OwnsOne(t => t.CommentsSyncInfo);
    }
}
