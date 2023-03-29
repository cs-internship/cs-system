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

        }

        [TestMethod]
        public async Task GitHubBadgesList_LoadSimple()
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

            var badgesFolderUrl =
                "https://github.com/cs-internship/cs-system/tree/main/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/github-sample-folder";
            var badges = await githubService.GetBadgesAsync(badgesFolderUrl);

            Assert.IsNotNull(badges);
            Assert.AreEqual(6, badges.Count);

            var badgeSystem = factory.CreateNew(badges);

        }
    }
}