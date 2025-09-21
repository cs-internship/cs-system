using Microsoft.Extensions.Logging;
using CrystaLearn.Client.Windows.Services;
using CrystaLearn.Client.Core.Services.HttpMessageHandlers;

namespace CrystaLearn.Client.Windows;

public static partial class Program
{
    public static void AddClientWindowsProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Services being registered here can get injected in windows project only.
        services.AddClientCoreProjectServices(configuration);

        services.AddScoped<IWebAuthnService, WindowsWebAuthnService>();
        services.AddScoped<IExceptionHandler, WindowsExceptionHandler>();
        services.AddScoped<IAppUpdateService, WindowsAppUpdateService>();
        services.AddScoped<IBitDeviceCoordinator, WindowsDeviceCoordinator>();

        services.AddScoped<HttpClient>(sp =>
        {
            var handlerFactory = sp.GetRequiredService<HttpMessageHandlersChainFactory>();
            var httpClient = new HttpClient(handlerFactory.Invoke())
            {
                BaseAddress = new Uri(configuration.GetServerAddress(), UriKind.Absolute)
            };
            if (sp.GetRequiredService<ClientWindowsSettings>().WebAppUrl is Uri origin)
            {
                httpClient.DefaultRequestHeaders.Add("X-Origin", origin.ToString());
            }
            return httpClient;
        });

        services.AddSingleton(sp => configuration);
        services.AddSingleton<IStorageService, WindowsStorageService>();
        services.AddSingleton<ILocalHttpServer, WindowsLocalHttpServer>();

        ClientWindowsSettings settings = new();
        configuration.Bind(settings);
        services.AddSingleton(sp =>
        {
            return settings;
        });
        services.AddSingleton(ITelemetryContext.Current!);
        services.AddSingleton<IPushNotificationService, WindowsPushNotificationService>();

        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ConfigureLoggers(configuration);
            loggingBuilder.AddEventSourceLogger();

            loggingBuilder.AddEventLog(options => configuration.GetRequiredSection("Logging:EventLog").Bind(options));
            if (string.IsNullOrEmpty(settings.ApplicationInsights?.ConnectionString) is false)
            {
                loggingBuilder.AddApplicationInsights(config =>
                {
                    config.TelemetryInitializers.Add(new WindowsAppInsightsTelemetryInitializer());
                    configuration.GetRequiredSection("ApplicationInsights").Bind(config);
                }, options => configuration.GetRequiredSection("Logging:ApplicationInsights").Bind(options));
            }
        });

        services.AddOptions<ClientWindowsSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
