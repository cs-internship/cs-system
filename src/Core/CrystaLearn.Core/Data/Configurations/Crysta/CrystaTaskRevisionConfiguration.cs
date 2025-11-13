using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskRevisionConfiguration : IEntityTypeConfiguration<CrystaTaskRevision>
{
    public void Configure(EntityTypeBuilder<CrystaTaskRevision> builder)
    {
        builder.HasIndex(r => r.AzureWorkItemId);
        builder.HasIndex(r => r.CrystaTaskId);
        builder.HasIndex(r => r.RevisionNumber);
        builder.HasIndex(r => r.ChangedDate);

        builder.HasOne(r => r.CrystaTask)
            .WithMany()
            .HasForeignKey(r => r.CrystaTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
