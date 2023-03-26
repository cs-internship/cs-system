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
            //throw new NotImplementedException();
            var client = new GitHubClient(new ProductHeaderValue("CS-System"));
            var repos = await client.Repository.GetAllForOrg("cs-internship");
            var repo = repos.First(r => r.Name == "cs-system");

            var refs = await client.Git.Reference.GetAll(repo.Id);

            //var main = refs.First(r => r.Ref.Contains("refs/heads/main"));
            var main = refs.First(r => r.Ref.Contains("refs/heads/main"));
            //var getCorrectUrl = url.IndexOf("/main", StringComparison.Ordinal);

            //if (getCorrectUrl < 0 || getCorrectUrl >= url.Length)
            //    return null;

            //var refUrl =
            //    $"refs/heads{url.Substring(getCorrectUrl)}";

            //var xx = await client.Git.Tree.GetRecursive(repo.Id, main.Ref);
            ////https://github.com/cs-internship/cs-system/tree/main/src/CrystallineSociety/Shared/CrystallineSociety.Shared.Test/BadgeSystem/SampleBadgeDocs/serialization-badge-sample
            //List<TreeItem> sample = new List<TreeItem>();
            //foreach (var item in xx.Tree)
            //{
            //    sample.Add(item);
            //}
            var getCorrectUrlForSrc = url.IndexOf("src", StringComparison.Ordinal);
            var folderName = url.Substring(getCorrectUrlForSrc);
            var contents = await client.Repository.Content.GetAllContents(repo.Id, folderName);
            var badgeFilePath = contents.FirstOrDefault(x => x.Name.Contains(".json"))?.Path;
            var badgeFile = await client.Repository.Content.GetAllContents(repo.Id, badgeFilePath);
            var badgeFileContent = badgeFile.FirstOrDefault()?.Content;
            var badge = new BadgeDto();
            if (badgeFileContent != null)
            {
                badge = BadgeUtilService.ParseBadge(badgeFileContent);
            }

            return badge;
        }
    }
}
