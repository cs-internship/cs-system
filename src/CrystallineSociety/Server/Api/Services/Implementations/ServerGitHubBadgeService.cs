using CrystallineSociety.Shared.Dtos.BadgeSystem;
using Octokit;

namespace CrystallineSociety.Server.Api.Services.Implementations
{
    public partial class ServerGitHubBadgeService : IGitHubBadgeService
    {
        [AutoInject]
        public IBadgeUtilService BadgeUtilService { get; set; }

        public async Task<List<BadgeDto>> GetBadgesAsync(string url)
        {
            // Todo: return the real list.
            return new List<BadgeDto>()
            {
                BadgeUtilService.ParseBadge($$"""{"code": "github-test-badge-a", "description": "from: {{url}}"}"""),
                BadgeUtilService.ParseBadge($$"""{"code": "github-test-badge-b", "description": "from: {{url}}"}"""),
            };
        }

        public async Task<BadgeDto> GetBadgeAsync(string url)
        {
            var client = new GitHubClient(new ProductHeaderValue("CS-System"));
            var repos = await client.Repository.GetAllForOrg("cs-internship");
            var repo = repos.First(r => r.Name == "cs-system");
            var urlSrcIndex = url.IndexOf("src", StringComparison.Ordinal);
            var folderPath = url[urlSrcIndex..];
            var folderContents = await client.Repository.Content.GetAllContents(repo.Id, folderPath);
            var badgeFilePath = folderContents.FirstOrDefault(x => x.Name.Contains(".json"))?.Path;
            var badgeFile = await client.Repository.Content.GetAllContents(repo.Id, badgeFilePath);
            var badge = new BadgeDto();
            var badgeFileContent = badgeFile?.FirstOrDefault()?.Content;

            if (badgeFileContent == null)
                return badge;

            try
            {
                badge = BadgeUtilService.ParseBadge(badgeFileContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return badge;
        }
    }
}
