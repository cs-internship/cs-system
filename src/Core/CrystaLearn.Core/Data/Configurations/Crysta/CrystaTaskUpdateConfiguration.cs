using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskUpdateConfiguration : IEntityTypeConfiguration<CrystaTaskUpdate>
{
    public void Configure(EntityTypeBuilder<CrystaTaskUpdate> builder)
    {
        builder.HasIndex(u => u.AzureWorkItemId);
        builder.HasIndex(u => u.CrystaTaskId);
        builder.HasIndex(u => u.Revision);
        builder.HasIndex(u => u.ChangedDate);

        builder.HasOne(u => u.CrystaTask)
            .WithMany()
            .HasForeignKey(u => u.CrystaTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(u => u.SyncInfo);
    }
}
