using CrystaLearn.Core.Models.Attachments;
using CrystaLearn.Core.Models.Chatbot;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.PushNotification;
using CrystaLearn.Server.Api.Data.Configurations;
using CrystaLearn.Server.Api.Models.Identity;
using Hangfire.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace CrystaLearn.Core.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options), IDataProtectionKeyContext
{
    public DbSet<UserSession> UserSessions { get; set; } = default!;

    public DbSet<PushNotificationSubscription> PushNotificationSubscriptions { get; set; } = default!;

    public DbSet<WebAuthnCredential> WebAuthnCredential { get; set; } = default!;

    public DbSet<SystemPrompt> SystemPrompts { get; set; } = default!;

    public DbSet<Attachment> Attachments { get; set; } = default!;

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

    public DbSet<CrystaTask> CrystaTasks { get; set; } = default!;

    public DbSet<CrystaTaskComment> CrystaTaskComments { get; set; } = default!;

    public DbSet<CrystaTaskUpdate> CrystaTaskUpdates { get; set; } = default!;

    public DbSet<CrystaTaskRevision> CrystaTaskRevisions { get; set; } = default!;

    public DbSet<CrystaProgram> CrystaPrograms { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.OnHangfireModelCreating("jobs");

        modelBuilder.HasDefaultSchema("CrystaLearn");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureIdentityTableNames(modelBuilder);

        ConfigureConcurrencyStamp(modelBuilder);
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


        configurationBuilder.Conventions.Add(_ => new PostgresPrimaryKeySequentialGuidDefaultValueConvention());

        base.ConfigureConventions(configurationBuilder);
    }

    private void ConfigureIdentityTableNames(ModelBuilder builder)
    {
        builder.Entity<User>()
            .ToTable("Users");

        builder.Entity<Role>()
            .ToTable("Roles");

        builder.Entity<UserRole>()
            .ToTable("UserRoles");

        builder.Entity<RoleClaim>()
            .ToTable("RoleClaims");

        builder.Entity<UserClaim>()
            .ToTable("UserClaims");

        builder.Entity<UserLogin>()
            .ToTable("UserLogins");

        builder.Entity<UserToken>()
            .ToTable("UserTokens");
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

    // This requires SQL Server 2025+
    public static readonly bool IsEmbeddingEnabled = false;
}
