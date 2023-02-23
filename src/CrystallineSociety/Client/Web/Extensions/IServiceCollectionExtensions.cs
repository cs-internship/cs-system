using CrystallineSociety.Client.Web.AppHooks;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddClientWebServices(this IServiceCollection services)
    {
        // Services being registered here can get injected in web (blazor web assembly & blzor server)
        //services.AddAppHook<ClientBadgeSystemAppHook>();

        return services;
    }
}
