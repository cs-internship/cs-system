using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CrystaLearn.Core.Tests.Infra;
public class TestBase
{
    protected IServiceProvider CreateServiceProvider(Action<IServiceCollection>? configServices = null)
    {

        var builder = Host.CreateApplicationBuilder(settings: new HostApplicationBuilderSettings
        {
            EnvironmentName = Environments.Development,
            ApplicationName = typeof(TestBase).Assembly.GetName().Name
        });

        AppEnvironment.Set(builder.Environment.EnvironmentName);
        
        builder.Configuration.AddSharedConfigurations();

        builder.AddCrystaServices();

        if (configServices is not null)
        {
            configServices(builder.Services);
        }

        var host = builder.Build();

        return host.Services;
    }
}
