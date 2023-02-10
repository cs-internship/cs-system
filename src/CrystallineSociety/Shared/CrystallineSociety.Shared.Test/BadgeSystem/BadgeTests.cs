using CrystallineSociety.Shared.Test.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using CrystallineSociety.Shared.Test.Utils;

namespace CrystallineSociety.Shared.Test.BadgeSystem
{
    [TestClass]
    public class BadgeTests : TestBase
    {
        public TestContext TestContext { get; set; } = default!;

            [TestMethod]
        public async Task Badge_LoadSimple()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var badgeService = testHost.Services.GetService<IBadgeService>();

            Assert.IsNotNull(badgeService);

            var specJson = await ResourceUtil.GetResourceAsync("doc_guru_badge_sample.spec.json");
            var badge = badgeService.Parse(specJson);

            Assert.IsNotNull(badge);
            Assert.IsNotNull(badge.Code);
            Assert.IsNotNull(badge.Description);



        }
    }
}