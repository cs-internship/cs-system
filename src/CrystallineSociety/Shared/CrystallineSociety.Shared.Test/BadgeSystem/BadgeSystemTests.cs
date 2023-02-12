using CrystallineSociety.Shared.Test.Infrastructure;
using CrystallineSociety.Shared.Test.Utils;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

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

            var badgeService = testHost.Services.GetService<IBadgeService>();

            Assert.IsNotNull(badgeService);

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            Assert.IsNotNull(badge);

            var badgeSystem = new BadgeSystemDto();
            badgeSystem.Badges.Add(badge);

            badgeService.BuildBadgeSystem(badgeSystem);

            Assert.IsNotNull(badgeSystem.Validations);
            Assert.IsTrue(badgeSystem.Validations.Any());

        }
    }
}