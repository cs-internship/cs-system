﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Test.Infrastructure
{
    public class TestBase
    {
        //protected IServiceProvider ServiceProvider { get; set; } = default!;
        //protected IServiceScope ServiceScope { get; set; } = default!;
        //protected IHost TestHost { get; set; } = default!;

        //public TestBase()
        //{
        //    TestHost = Host.CreateDefaultBuilder()
        //       .ConfigureServices((_, services) =>
        //       {
        //           services.AddAppServices();
        //       }
        //    ).Build();

        //    ServiceScope = TestHost.Services.CreateScope();
        //    ServiceProvider = ServiceScope.ServiceProvider;
        //}

        //protected IHost CreateHost(Action<IServiceCollection> additionalServices)
        //{
        //    return Host.CreateDefaultBuilder()
        //       .ConfigureServices((_, services) =>
        //       {
        //           services.AddAppServices();
        //           additionalServices(services);
        //       }
        //    ).Build();
        //}


        //public void Dispose()
        //{
        //    TestHost.Dispose();
        //    ServiceScope.Dispose();
        //}
    }
}
