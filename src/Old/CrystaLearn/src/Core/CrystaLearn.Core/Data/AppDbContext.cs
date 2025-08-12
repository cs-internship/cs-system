using CrystaLearn.Core.Data.Configurations;
using CrystaLearn.Core.Models.Attachments;
using CrystaLearn.Core.Models.Chatbot;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.PushNotification;
using Hangfire.EntityFrameworkCore;

namespace CrystaLearn.Core.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>(options)
{
    public DbSet<UserSession> UserSessions { get; set; } = default!;

    public DbSet<PushNotificationSubscription> PushNotificationSubscriptions { get; set; } = default!;

    public DbSet<WebAuthnCredential> WebAuthnCredential { get; set; } = default!;

    public DbSet<SystemPrompt> SystemPrompts { get; set; } = default!;

    public DbSet<Attachment> Attachments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.OnHangfireModelCreating("jobs");

        //modelBuilder.HasSequence<int>("ProductShortId")
        //    .StartsAt(10_051) // There are 50 products added by ProductConfiguration.cs
        //    .IncrementsBy(1);

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureIdentityTableNames(modelBuilder);

        ConfigureConcurrencyStamp(modelBuilder);

        ConfigureCrysta(modelBuilder);
    }
    private void ConfigureCrysta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .OwnsOne(o => o.SyncInfo);

        modelBuilder.Entity<CrystaProgram>()
            .OwnsOne(o => o.BadgeSyncInfo);
        modelBuilder.Entity<CrystaProgram>()
                    .OwnsOne(o => o.DocumentSyncInfo);

    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            SetConcurrencyStamp();

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer
            return base.SaveChanges(acceptAllChangesOnSuccess);
#pragma warning restore NonAsyncEFCoreMethodsUsageAnalyzer
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
            SetConcurrencyStamp();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(nameof(AppStrings.UpdateConcurrencyException), exception);
        }
    }

    private void SetConcurrencyStamp()
    {
        ChangeTracker.DetectChanges();

        foreach (var entityEntry in ChangeTracker.Entries().Where(e => e.State is EntityState.Modified or EntityState.Deleted))
        {
            if (entityEntry.CurrentValues.TryGetValue<object>("ConcurrencyStamp", out var currentConcurrencyStamp) is false
                || currentConcurrencyStamp is not byte[])
                continue;

            // https://github.com/dotnet/efcore/issues/35443
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
            .ToTable("User");

        builder.Entity<Role>()
            .ToTable("Role");

        builder.Entity<UserRole>()
            .ToTable("UserRole");

        builder.Entity<RoleClaim>()
            .ToTable("RoleClaim");

        builder.Entity<UserClaim>()
            .ToTable("UserClaim");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("UserLogin");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("UserToken");

        builder.Entity<UserSession>()
          .ToTable("UserSession");
    }

    private void ConfigureConcurrencyStamp(ModelBuilder modelBuilder)
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
