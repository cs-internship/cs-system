using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using Microsoft.Extensions.DependencyInjection;
using IBadgeSystemService = CrystallineSociety.Server.Services.Contracts.IBadgeSystemService;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

public partial class BadgeSystemFactory
{
    [AutoInject] private IServiceScopeFactory ServiceScopeFactory { get; set; }
    private IBadgeSystemService? DefaultBadgeSystem { get; set; }
    public IBadgeSystemService CreateNew(BadgeBundleDto bundle)
    {
        using IServiceScope scope = ServiceScopeFactory.CreateScope();
        var badgeService = scope.ServiceProvider.GetRequiredService<IBadgeSystemService>();
        badgeService.Build(bundle);
        return badgeService;
    }

    public IBadgeSystemService CreateNew(List<BadgeDto> badges)
    {
        var bundle = new BadgeBundleDto(badges);
        using IServiceScope scope = ServiceScopeFactory.CreateScope();
        var badgeService = scope.ServiceProvider.GetRequiredService<IBadgeSystemService>();
        badgeService.Build(bundle);
        return badgeService;
    }

    public IBadgeSystemService Default()
    {
        return DefaultBadgeSystem ?? throw new InvalidOperationException("Default badge system is not set yet.");
    }

    public void SetCurrentBadgeSystem(BadgeBundleDto bundle)
    {
        DefaultBadgeSystem = CreateNew(bundle);
    }
}
