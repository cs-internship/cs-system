using CrystallineSociety.Shared.Services.Implementations;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows, and Mac)

        services.AddLocalization();

        services.AddAuthorizationCore();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IBadgeService, BadgeService>();
    }
}
