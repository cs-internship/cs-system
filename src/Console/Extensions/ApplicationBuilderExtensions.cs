using Microsoft.Extensions.Hosting;

namespace CrystaLearn.Console.Extensions;
public static class ApplicationBuilderExtensions
{
    public static void AddConsoleServices(this IHostApplicationBuilder builder)
    {
        var env = builder.Environment;
        var services = builder.Services;
        var configuration = builder.Configuration;

        //services.AddTransient<IDocumentRepository, DocumentRepositoryFake>();
        //services.AddTransient<ICrystaProgramRepository, CrystaProgramRepositoryFake>();
    }
}
