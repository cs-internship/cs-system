using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using Microsoft.Extensions.Configuration;
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

            var badgeUrl =
                "https://github.com/hootanht/cs-system/blob/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample/spec-badge.json";

            var badge = await githubService.GetBadgeAsync(badgeUrl);

            Assert.IsNotNull(badge);
        }

        [TestMethod]
        public async Task GitHubBadge_LoadByFolderAddress_FileNotFoundException()
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
                "https://github.com/hootanht/cs-system/blob/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample";


            var exception = await Assert.ThrowsExceptionAsync<FileNotFoundException>(async () =>
            {
                await githubService.GetBadgeAsync(badgesFolderUrl);
            });

            Assert.IsNotNull(exception);
            Assert.AreEqual($"Badge file not found in: {badgesFolderUrl}", exception.Message);
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
        }


        [TestMethod]
        public async Task GetBadgeAsync_InvalidRepoIdOrSha_ThrowNotFoundException()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            long repoId = 619299373; //correct: 619299373
            string invalidSha =
                "cf74dc41bb1a474fe7ff3e2624aae055ee5338ec"; //correct: cf74dc41bb1a474fe7ff3e2624aae055ee5338eb

            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
            {
                await githubService.GetBadgeAsync(repoId, invalidSha);
            });
        }

        [TestMethod]
        public async Task GetBadgeAsync_ValidRepoIdAndSha_AreEqual()
        {
            var testHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    {
                        services.AddSharedServices();
                        services.AddServerServices();
                    }
                ).Build();

            var githubService = testHost.Services.GetRequiredService<IGitHubBadgeService>();

            long repoId = 619299373;
            string sha = "cf74dc41bb1a474fe7ff3e2624aae055ee5338eb";

            var result = await githubService.GetBadgeAsync(repoId, sha);
            Assert.AreEqual("doc-beginner", result.Code);
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
                "https://github.com/hootanht/cs-system/tree/feature/initial-get-badge-system/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/github-sample-folder";
            var badges = await githubService.GetBadgesAsync(badgesFolderUrl);

            Assert.IsNotNull(badges);
            Assert.AreEqual(6, badges.Count);

            var badgeSystem = factory.CreateNew(badges);
        }
    }
}