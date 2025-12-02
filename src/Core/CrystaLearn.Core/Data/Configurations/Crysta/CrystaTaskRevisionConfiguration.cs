using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskRevisionConfiguration : IEntityTypeConfiguration<CrystaTaskRevision>
{
    public void Configure(EntityTypeBuilder<CrystaTaskRevision> builder)
    {
        builder.HasIndex(r => r.ProviderTaskId);
        builder.HasIndex(r => r.CrystaTaskId);
        builder.HasIndex(r => r.Revision);
        builder.HasIndex(r => r.ChangedDate);
        builder.HasIndex(r => r.ProjectId);
        builder.HasIndex(r => r.State);
        builder.HasIndex(r => r.CreatedDate);

        builder.HasOne(r => r.CrystaTask)
            .WithMany()
            .HasForeignKey(r => r.CrystaTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.AssignedTo)
            .WithMany()
            .HasForeignKey(r => r.AssignedToId);

        builder.HasOne(r => r.CompletedBy)
            .WithMany()
            .HasForeignKey(r => r.CompletedById);

        builder.HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedById);

        builder.HasOne(r => r.CrystaProgram)
            .WithMany()
            .HasForeignKey(r => r.CrystaProgramId);

        builder.OwnsOne(u => u.WorkItemSyncInfo, sync =>
        {
            sync.HasIndex(s => s.SyncId).IsUnique();
        });
        builder.OwnsOne(r => r.RevisionsSyncInfo);
        builder.OwnsOne(r => r.UpdatesSyncInfo);
        builder.OwnsOne(r => r.CommentsSyncInfo);
       
    }
}
