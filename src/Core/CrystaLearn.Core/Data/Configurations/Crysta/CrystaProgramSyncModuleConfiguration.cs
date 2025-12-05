using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Data.Configurations.Crysta;

public class CrystaProgramSyncModuleConfiguration : IEntityTypeConfiguration<CrystaProgramSyncModule>
{
    public void Configure(EntityTypeBuilder<CrystaProgramSyncModule> builder)
    {
        builder.HasOne(m => m.CrystaProgram)
               .WithMany()
               .HasForeignKey(m => m.CrystaProgramId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.CrystaProgramId);
        builder.HasIndex(m => m.ModuleType);

        builder.OwnsOne(m => m.SyncInfo, syncInfo =>
        {
            syncInfo.Property(s => s.SyncId).HasMaxLength(100);
            syncInfo.Property(s => s.ContentHash).HasMaxLength(100);
            syncInfo.Property(s => s.SyncGroup).HasMaxLength(100);
            syncInfo.Property(s => s.LastSyncOffset).HasMaxLength(40);
        });
    }
}
