using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Server.Api.Data.Configurations;
using CrystaLearn.Core.Models.PushNotification;
using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Server.Api.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options), IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

    public DbSet<UserSession> UserSessions { get; set; } = default!;

    public DbSet<PushNotificationSubscription> PushNotificationSubscriptions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureIdentityTableNames(modelBuilder);

        ConcurrencyStamp(modelBuilder);

        ConfigureCrysta(modelBuilder);
    }

    private void ConfigureCrysta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .OwnsOne(o=>o.SyncInfo);
        
        modelBuilder.Entity<CrystaProgram>()
            .OwnsOne(o => o.BadgeSyncInfo);
        modelBuilder.Entity<CrystaProgram>()
                    .OwnsOne(o => o.DocumentSyncInfo);

    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            ReplaceOriginalConcurrencyStamp();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(nameof(AppStrings.UpdateConcurrencyException), exception);
        }
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            ReplaceOriginalConcurrencyStamp();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(nameof(AppStrings.UpdateConcurrencyException), exception);
        }
    }

    /// <summary>
    /// https://github.com/dotnet/efcore/issues/35443
    /// </summary>
    private void ReplaceOriginalConcurrencyStamp()
    {
        ChangeTracker.DetectChanges();

        foreach (var entityEntry in ChangeTracker.Entries().Where(e => e.State is EntityState.Modified))
        {
            if (entityEntry.CurrentValues.TryGetValue<object>("ConcurrencyStamp", out var currentConcurrencyStamp) is false
                || currentConcurrencyStamp is not byte[])
                continue;

            entityEntry.OriginalValues.SetValues(new Dictionary<string, object> { { "ConcurrencyStamp", currentConcurrencyStamp } });
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {


            configurationBuilder.Conventions.Add(_ => new SqlServerPrimaryKeySequentialGuidDefaultValueConvention());

        base.ConfigureConventions(configurationBuilder);
    }

    private void ConfigureIdentityTableNames(ModelBuilder builder)
    {
        builder.Entity<User>()
            .ToTable("Users");

        builder.Entity<Role>()
            .ToTable("Roles");

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("UserRoles");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("UserLogins");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("UserTokens");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims");

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("UserClaims");
    }

    private void ConcurrencyStamp(ModelBuilder modelBuilder)
    {

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.Name is "ConcurrencyStamp" && p.PropertyInfo?.PropertyType == typeof(byte[])))
            {
                var builder = new PropertyBuilder(property);
                builder.IsConcurrencyToken()
                    .IsRowVersion();

            }
        }
    }
}
