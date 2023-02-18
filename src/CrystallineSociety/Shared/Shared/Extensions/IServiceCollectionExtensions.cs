using CrystallineSociety.Shared.Services.Implementations;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows, and Mac)

        services.AddLocalization();

        services.AddAuthorizationCore();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IBadgeUtilService, BadgeUtilService>();
        services.AddTransient<IBadgeSystemService, BadgeSystemService>();
        services.AddSingleton<BadgeSystemFactory>();
        services.AddTransient<IBadgeSystemValidator, RequirementsHaveValidBadgesValidator>();
    }

    public static void AddAppHook<T>(this IServiceCollection services) where T : class, IAppHook
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows, and Mac)
        services.AddTransient<IAppHook, T>();
    }
}
