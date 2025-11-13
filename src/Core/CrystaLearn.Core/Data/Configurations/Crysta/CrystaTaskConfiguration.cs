using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskConfiguration : IEntityTypeConfiguration<CrystaTask>
{
    public void Configure(EntityTypeBuilder<CrystaTask> builder)
    {
        builder.HasIndex(t => t.ProviderTaskId);
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.State);
        builder.HasIndex(t => t.CreatedDate);
        builder.HasIndex(t => t.ChangedDate);

        builder.OwnsOne(t => t.WorkItemSyncInfo);
        builder.OwnsOne(t => t.RevisionsSyncInfo);
        builder.OwnsOne(t => t.UpdatesSyncInfo);
        builder.OwnsOne(t => t.CommentsSyncInfo);
    }
}
