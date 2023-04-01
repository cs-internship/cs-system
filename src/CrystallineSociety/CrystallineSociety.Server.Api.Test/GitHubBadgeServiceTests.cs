using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using Microsoft.Extensions.Hosting;
using Octokit;

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
                "https://github.com/hootanht/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample";
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
                "https://github.com/cs-internship/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/github-sample-folder";
            var badges = await githubService.GetBadgesAsync(badgesFolderUrl);

            Assert.IsNotNull(badges);
            Assert.AreEqual(6, badges.Count);

            var badgeSystem = factory.CreateNew(badges);

        }

        [TestMethod]
        public async Task GetBadgeAsync_IncorrectBranchName_ThrowResourceNotFoundException()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            var badgeUrl =
                "https://github.com/hootanht/cs-system/tree/feature-initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample";

            var ex = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await githubService.GetBadgeAsync(badgeUrl);
            });

            Assert.IsNotNull(ex);
            Assert.AreEqual($"Unable to locate branchName: {badgeUrl}", ex.Message);
        }

        [TestMethod]
        public async Task GetBadgeAsync_IncorrectOrganizationOrRepositoryName_ThrowOctokitNotFoundException()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            var badgeUrl =
                "https://github.com/hootanhtbug/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample";

            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await githubService.GetBadgeAsync(badgeUrl);
            });

            Assert.IsNotNull(ex);
            Assert.AreEqual($"Not Found", ex.Message);
        }

        [TestMethod]
        public async Task GetBadgeAsync_InvalidBadgeFileName_ThrowFileNotFoundException()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            var badgeUrl =
                "https://github.com/hootanht/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample/spec.json";

            var exception = await Assert.ThrowsExceptionAsync<FileNotFoundException>(async () =>
            {
                await githubService.GetBadgeAsync(badgeUrl);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual($"Badge file not found in: {badgeUrl}", exception.Message);
        }

        [TestMethod]
        public async Task GetBadgeAsync_UnparsableBadgeFileFormat_ThrowFormatException()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            var badgeUrl =
                "https://github.com/hootanht/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample/spec-empty-badge.json";

            var exception = await Assert.ThrowsExceptionAsync<FormatException>(async () =>
            {
                await githubService.GetBadgeAsync(badgeUrl);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual($"Can not parse badge with url: '{badgeUrl}'", exception.Message);
        }
    }
}