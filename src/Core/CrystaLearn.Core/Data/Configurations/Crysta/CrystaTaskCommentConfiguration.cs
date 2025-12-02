using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaTaskCommentConfiguration : IEntityTypeConfiguration<CrystaTaskComment>
{
    public void Configure(EntityTypeBuilder<CrystaTaskComment> builder)
    {
        builder.HasIndex(c => c.ProviderTaskId);
        builder.HasIndex(c => c.CrystaTaskId);
        builder.HasIndex(c => c.CreatedDate);

        builder.HasOne(c => c.CrystaTask)
            .WithMany()
            .HasForeignKey(c => c.CrystaTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(u => u.SyncInfo, sync =>
        {
            sync.HasIndex(s => s.SyncId).IsUnique();
        });
    }
}
