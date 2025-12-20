using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaDocumentConfiguration : IEntityTypeConfiguration<CrystaDocument>
{
    public void Configure(EntityTypeBuilder<CrystaDocument> builder)
    {
        builder.HasIndex(d => d.Code).IsUnique();
        builder.HasIndex(d => d.IsActive);
        builder.HasIndex(d => d.CrystaProgramId);

        builder.HasOne(d => d.CrystaProgram)
            .WithMany()
            .HasForeignKey(d => d.CrystaProgramId);

        builder.OwnsOne(d => d.SyncInfo, sync =>
        {
            sync.HasIndex(s => s.SyncId).IsUnique();
        });
    }
}
