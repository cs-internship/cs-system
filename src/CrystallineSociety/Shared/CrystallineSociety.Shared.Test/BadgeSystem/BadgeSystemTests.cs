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
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                       services.AddTestServices();
                                   }
                               ).Build();

            var badgeService = testHost.Services.GetRequiredService<IBadgeUtilService>();
            var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            Assert.IsNotNull(badge);

            var bundle = new BadgeBundleDto();
            bundle.Badges.Add(badge);

            var badgeSystem = factory.CreateNew(bundle);

            Assert.IsNotNull(badgeSystem.Validations);
            Assert.IsTrue(badgeSystem.Validations.Any());

        }

        //[TestMethod]
        //public async Task GetLearnerBadges_Simple()
        //{
        //    var testHost = Host.CreateDefaultBuilder()
        //                       .ConfigureServices((_, services) =>
        //                           {
        //                               services.AddSharedServices();
        //                               services.AddTestServices();
        //                           }
        //                       ).Build();

        //    var badgeService = testHost.Services.GetRequiredService<IBadgeUtilService>();
        //    var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

        //    var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
        //    var badge = badgeService.ParseBadge(specJson);

        //    Assert.IsNotNull(badge);

        //    var bundle = new BadgeBundleDto();
        //    bundle.Badges.Add(badge);

        //    var badgeSystem = factory.CreateNew(bundle);

        //    Assert.IsNotNull(badgeSystem.Validations);
        //    Assert.IsTrue(badgeSystem.Validations.Any());

        //}
    }
}