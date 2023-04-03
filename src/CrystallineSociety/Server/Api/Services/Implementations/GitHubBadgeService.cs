using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using Octokit;

namespace CrystallineSociety.Server.Api.Services.Implementations
{
    public partial class GitHubBadgeService : IGitHubBadgeService
    {
        [AutoInject] public IBadgeUtilService BadgeUtilService { get; set; }
        [AutoInject] public GitHubClient GitHubClient { get; set; }
        public async Task<List<BadgeDto>> GetBadgesAsync(string folderUrl)
        {
            var lightBadges = await GetLightBadgesAsync(folderUrl);

            var badges = new List<BadgeDto>();

            foreach (var lightBadge in lightBadges)
            {
                if (Path.GetExtension(lightBadge.Url) != ".json")
                    continue;

                var badgeDto = await GetBadgeAsync(
                    lightBadge.RepoId ?? throw new Exception("RepoId of light badge is null."), 
                    lightBadge.Sha ?? throw new Exception("Sha of light badge is null.")
                    );

                badges.Add(badgeDto);
            }

            return badges;
        }

        public async Task<List<BadgeDto>> GetLightBadgesAsync(string url)
        {
            var repos = await GitHubClient.Repository.GetAllForOrg("cs-internship");
            var repo = repos.First(r => r.Name == "cs-system");
            var lastSegment = GetLastSegmentFromUrl(url, out var parentFolderPath);
            var repositoryId = repo.Id;
            var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId, parentFolderPath);
            var folderSha = folderContents?.First(f => f.Name == lastSegment).Sha;
            var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, folderSha);
            return allContents.Tree
                            .Select(t => new BadgeDto { RepoId = repositoryId, Sha = t.Sha, Url = t.Url })
                            .ToList();
        }

        public async Task<BadgeDto> GetBadgeAsync(long repositoryId, string sha)
        {
            var badgeBlob = await GitHubClient.Git.Blob.Get(repositoryId, sha);

            var bytes = Convert.FromBase64String(badgeBlob.Content);
            var badgeContent = Encoding.UTF8.GetString(bytes);
            var badgeDto = BadgeUtilService.ParseBadge(badgeContent);
            return badgeDto;
        }

        private static GitHubClient CreateClient()
        {
            return new GitHubClient(new ProductHeaderValue("CS-System"));
        }

        public async Task<BadgeDto> GetBadgeAsync(string badgeUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(badgeUrl);

            var repo = await GitHubClient.Repository.Get(orgName, repoName);

            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);
            var branchName = GetBranchNameFromUrl(badgeUrl, refs) ??
                             throw new ResourceNotFoundException($"Unable to locate branchName: {badgeUrl}");
            var branchRef = refs.First(r => r.Ref.Contains($"refs/heads/{branchName}"));
            var folderPath = GetRelativeFolderPath(badgeUrl);
            var folderContents =
                await GitHubClient.Repository.Content.GetAllContentsByRef(repo.Id, folderPath, branchRef.Ref);

            var badgeFilePath =
                folderContents.FirstOrDefault(x => x.Name.EndsWith("-badge.json"))?.Path
                ?? throw new FileNotFoundException($"Badge file not found in: {badgeUrl}");
            var contents = await GitHubClient.Repository.Content.GetAllContentsByRef(repo.Id, badgeFilePath, branchRef.Ref);
            var badgeFile = contents!.First();
            
            var badgeFileContent = badgeFile.Content;
            
            try
            {
                var badge = BadgeUtilService.ParseBadge(badgeFileContent);
                return badge;
            }
            catch (Exception exception)
            {
                throw new FormatException($"Can not parse badge with badgeUrl: '{badgeUrl}'", exception);
            }

        }

        private static string GetRelativeFolderPath(string url)
        {
            var urlSrcIndex = url.IndexOf("src", StringComparison.Ordinal);
            var folderPath = url[urlSrcIndex..];
            return folderPath;
        }

        private static string? GetBranchNameFromUrl(string url, IReadOnlyList<Reference> refs)
        {
            var uri = new Uri(url);
            var afterTreeSegments = String.Join("", uri.Segments[4..]);
            foreach (var reference in refs)
            {

                var branchInRefWithEndingSlash = $"{Regex.Replace(reference.Ref, @"^[^/]+/[^/]+/", "")}/";
                if (afterTreeSegments.StartsWith(branchInRefWithEndingSlash))
                {
                    return branchInRefWithEndingSlash.TrimEnd('/');
                }
            }

            return null;
        }

        private static string GetLastSegmentFromUrl(string url, out string parentFolderPath)
        {
            var uri = new Uri(url);
            var lastSegment = uri.Segments.Last().TrimEnd('/');
            var parentFolderUrl = uri.GetLeftPart(UriPartial.Authority) +
                                  string.Join("", uri.Segments.Take(uri.Segments.Length - 1));
            var urlSrcIndex = parentFolderUrl.IndexOf("src", StringComparison.Ordinal);
            parentFolderPath = parentFolderUrl[urlSrcIndex..];

            return lastSegment;
        }

        private static (string org, string repo) GetRepoAndOrgNameFromUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var org = segments[1].TrimEnd('/');
            var repo = segments[2].TrimEnd('/');

            return (org, repo);
        }
    }
}