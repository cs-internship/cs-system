using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using Microsoft.Extensions.Hosting;

namespace CrystallineSociety.Server.Api.Test
{
    [TestClass]
    public class GitHubBadgeServiceTests
    {
        public TestContext TestContext { get; set; } = default!;

        [TestMethod]
        public async Task GitHubBadge_LoadSimple()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                       services.AddServerServices();
                                   }
                               ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();
            var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

            var badgeUrl =
                "https://github.com/cs-internship/cs-system/tree/main/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample";
            var badge = await githubService.GetBadgeAsync(badgeUrl);

            Assert.IsNotNull(badge);

            var bundle = new BadgeBundleDto();
            bundle.Badges.Add(badge);

            var badgeSystem = factory.CreateNew(bundle);

            Assert.IsNotNull(badgeSystem.Validations);
            Assert.IsFalse(badgeSystem.Validations.Any());

        }
    }
}