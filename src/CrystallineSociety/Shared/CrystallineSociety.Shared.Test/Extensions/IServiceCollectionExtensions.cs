using CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Test.Fake;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddTestServices(this IServiceCollection services)
    {
        services.AddSingleton<ILeanerService>(new FakeLearnerService(new List<LearnerDto> { }));
    }
}