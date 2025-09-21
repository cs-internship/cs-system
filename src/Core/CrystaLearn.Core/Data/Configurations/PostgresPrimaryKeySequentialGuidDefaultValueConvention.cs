using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CrystaLearn.Server.Api.Data.Configurations;

public partial class PostgresPrimaryKeySequentialGuidDefaultValueConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var prop in entityType.GetKeys().SelectMany(k => k.Properties).Where(p => p.ClrType == typeof(Guid)))
            {
                prop.SetDefaultValueSql("gen_random_uuid()");
            }
        }
    }
}
