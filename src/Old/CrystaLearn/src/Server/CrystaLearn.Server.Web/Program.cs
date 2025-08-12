using CrystaLearn.Server.Web.Services;
using CrystaLearn.Client.Core.Services.Contracts;

namespace CrystaLearn.Server.Web;

public static partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(options: new()
        {
            Args = args,
        });

        AppEnvironment.Set(builder.Environment.EnvironmentName);

        builder.Configuration.AddClientConfigurations(clientEntryAssemblyName: "CrystaLearn.Client.Web");

        builder.WebHost.UseSentry(configureOptions: options => builder.Configuration.GetRequiredSection("Logging:Sentry").Bind(options));

        // The following line (using the * in the URL), allows the emulators and mobile devices to access the app using the host IP address.
        if (builder.Environment.IsDevelopment() && AppPlatform.IsWindows)
        {
            builder.WebHost.UseUrls("http://localhost:5000", "http://*:5000");
        }

        builder.AddServerWebProjectServices();

        var app = builder.Build();

        AppDomain.CurrentDomain.UnhandledException += (_, e) => LogException(e.ExceptionObject, reportedBy: nameof(AppDomain.UnhandledException), app);
        TaskScheduler.UnobservedTaskException += (_, e) => { LogException(e.Exception, reportedBy: nameof(TaskScheduler.UnobservedTaskException), app); e.SetObserved(); };


        app.ConfigureMiddlewares();

        #if Development
        _ = ScssCompilerService.WatchScssFiles(app);
#endif
        
        await app.RunAsync();
    }

    private static void LogException(object? error, string reportedBy, WebApplication app)
    {
        if (error is Exception exp)
        {
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<IExceptionHandler>().Handle(exp, parameters: new()
            {
                { nameof(reportedBy), reportedBy }
            }, displayKind: AppEnvironment.IsDev() ? ExceptionDisplayKind.NonInterrupting : ExceptionDisplayKind.None);
        }
        else
        {
            _ = Console.Error.WriteLineAsync(error?.ToString() ?? "Unknown error");
        }
    }
}
