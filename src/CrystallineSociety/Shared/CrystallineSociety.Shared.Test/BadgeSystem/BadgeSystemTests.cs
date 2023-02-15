using CrystallineSociety.Shared.Test.Infrastructure;
using CrystallineSociety.Shared.Test.Utils;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Shared.Test.BadgeSystem
{
    [TestClass]
    public class BadgeSystemTests : TestBase
    {
        public TestContext TestContext { get; set; } = default!;

        [TestMethod]
        public async Task Badge_LoadSimple()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) => { services.AddSharedServices(); }
                               ).Build();

            var badgeService = testHost.Services.GetRequiredService<IBadgeService>();
            var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            Assert.IsNotNull(badge);

            var bundle = new BadgeBundleDto();
            bundle.Badges.Add(badge);

            var badgeSystem = factory.CreateNew(bundle);

            Assert.IsNotNull(bundle.Validations);
            Assert.IsTrue(bundle.Validations.Any());

        }
    }
}