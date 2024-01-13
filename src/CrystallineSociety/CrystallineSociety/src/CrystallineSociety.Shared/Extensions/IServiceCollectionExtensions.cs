using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations;
using CrystallineSociety.Shared.Services.Implementations.ProgramDocument;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows and macOS)

        services.TryAddTransient<IDateTimeProvider, DateTimeProvider>();

        // Define authorization policies here to seamlessly integrate them across various components,
        // including web api actions and razor pages using Authorize attribute, AuthorizeView in razor pages, and programmatically in C# for enhanced security and access control.
        services.AddAuthorizationCore(options => options.AddPolicy("AdminOnly", authPolicyBuilder => authPolicyBuilder.RequireRole("Admin")));

        services.AddLocalization();
        
        services.AddTransient<IBadgeUtilService, BadgeUtilService>();
        services.AddTransient<IProgramDocumentUtilService,ProgramDocumentUtilService>();
        services.AddTransient<IBadgeSystemService, BadgeSystemService>();
        services.AddSingleton<BadgeSystemFactory>();
        services.AddTransient<IBadgeSystemValidator, RequirementsHaveValidBadgesValidator>();
        services.AddTransient<IBadgeSystemValidator, BadgeMustHaveValidNameValidator>();
        services.AddTransient<IBadgeSystemValidator, RepeatDependencyValidator>();
        services.AddTransient<IBadgeSystemValidator, RepeatedApprovingStepsValidator>();
        services.AddTransient<IBadgeSystemValidator, RepeatedActivityRequirementValidator>();
        services.AddTransient<IBadgeSystemValidator, RepeatedAppraisalMethodValidator>();
        services.AddSingleton<AppStateDto, AppStateDto>();

        return services;
    }
    
    public static void AddAppHook<T>(this IServiceCollection services) where T : class, IAppHook
    {
        // Services being registered here can get injected everywhere (Api, Web, Android, iOS, Windows, and Mac)
        services.AddSingleton<IAppHook, T>();
    }
}
