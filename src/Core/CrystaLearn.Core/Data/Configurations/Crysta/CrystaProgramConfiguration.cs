using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaProgramConfiguration : IEntityTypeConfiguration<CrystaProgram>
{
    public void Configure(EntityTypeBuilder<CrystaProgram> builder)
    {
        builder.HasIndex(p => p.Code).IsUnique();
        builder.HasIndex(p => p.IsActive);

        builder.OwnsOne(p => p.DocumentSyncInfo);
        builder.OwnsOne(p => p.BadgeSyncInfo);
    }
}
