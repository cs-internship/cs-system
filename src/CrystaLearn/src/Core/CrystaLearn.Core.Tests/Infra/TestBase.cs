using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Extensions;
using Microsoft.Extensions.Hosting;

namespace CrystaLearn.Core.Tests.Infra;
public class TestBase
{
    protected IServiceProvider CreateServiceProvider(Action<IServiceCollection>? configServices = null)
    {
        var builder = new HostApplicationBuilder();
        builder.AddCrystaServices();

        if (configServices is not null)
        {
            configServices(builder.Services);
        }

        var host = builder.Build();

        return host.Services;
    }
}
