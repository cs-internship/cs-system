namespace CrystaLearn.Server.Api;

public static partial class Program
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0#middleware-order
    /// </summary>
    private static void ConfigureMiddlewares(this WebApplication app)
    {
        var configuration = app.Configuration;
        var env = app.Environment;

        ServerApiSettings settings = new();
        configuration.Bind(settings);

        app.UseAppForwardedHeaders();

        app.UseLocalization();

        app.UseExceptionHandler();

        if (env.IsDevelopment() is false)
        {
            app.UseHttpsRedirection();
            app.UseResponseCompression();

            app.UseHsts();
            app.UseXContentTypeOptions();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
        }

        if (env.IsDevelopment())
        {
            app.UseDirectoryBrowser();
        }

        app.UseStaticFiles();

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseOutputCache();

        app.UseAntiforgery();

        app.MappAppHealthChecks();

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.InjectJavascript($"/scripts/swagger-utils.js?v={Environment.TickCount64}");
        });

        app.UseHangfireDashboard(options: new()
        {
            DarkModeEnabled = true,
            Authorization = [new HangfireDashboardAuthorizationFilter()]
        });

        // Configure recurring jobs
        ConfigureHangfireRecurringJobs();

        app.MapGet("/api/minimal-api-sample/{routeParameter}", [AppResponseCache(MaxAge = 3600 * 24)] (string routeParameter, [FromQuery] string queryStringParameter) => new
        {
            RouteParameter = routeParameter,
            QueryStringParameter = queryStringParameter
        }).WithTags("Test").CacheOutput("AppResponseCachePolicy");

        app.MapHub<SignalR.AppHub>("/app-hub", options => options.AllowStatefulReconnects = true);

        app.MapControllers()
           .RequireAuthorization()
           .CacheOutput("AppResponseCachePolicy");
    }

    private static void ConfigureHangfireRecurringJobs()
    {
        // Schedule CrystaProgram sync to run every 5 hours
        RecurringJob.AddOrUpdate<CrystaLearn.Core.Services.Jobs.CrystaProgramSyncJobRunner>(
            recurringJobId: "crysta-program-sync",
            methodCall: x => x.RunSyncForAllModules(CancellationToken.None),
            cronExpression: "*/1 * * * *");
    }
}
