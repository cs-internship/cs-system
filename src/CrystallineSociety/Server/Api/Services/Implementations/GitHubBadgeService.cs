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
                if (lightBadge.Url is null)
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
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(url);
            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);

            var lastSegment = GetLastSegmentFromUrl(url,refs,out var parentFolderPath);
            var repositoryId = repo.Id;

            // ToDo: Handle if the given url is a file url instead of a folder url which is required here.
            var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId, parentFolderPath);
            var folderSha = folderContents?.First(f => f.Name == lastSegment).Sha;
            var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, folderSha);

            return allContents.Tree
                .Where(t=>t.Type == TreeType.Blob )
                .Select(t => new BadgeDto { RepoId = repositoryId, Sha = t.Sha, Url = t.Url })
                .ToList();
        }

        public async Task<BadgeDto> GetBadgeAsync(string badgeUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(badgeUrl);
            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);
            var branchName = GetBranchNameFromUrl(badgeUrl, refs) ??
                             throw new ResourceNotFoundException($"Unable to locate branchName: {badgeUrl}");

            var branchRef = refs.First(r => r.Ref.Contains($"refs/heads/{branchName}"));
            var badgeFilePath = GetRelativePath(badgeUrl.EndsWith("-badge.json")
                ? badgeUrl
                : throw new FileNotFoundException($"Badge file not found in: {badgeUrl}"), refs);
            
            var badgeFileContentByte =
                await GitHubClient.Repository.Content.GetRawContentByRef(orgName, repoName, badgeFilePath, branchRef.Ref);

            var badgeFileContent = Encoding.UTF8.GetString(badgeFileContentByte);

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

        public async Task<BadgeDto> GetBadgeAsync(long repositoryId, string sha)
        {
            var badgeBlob = await GitHubClient.Git.Blob.Get(repositoryId, sha);

            var bytes = Convert.FromBase64String(badgeBlob.Content);
            var badgeContent = Encoding.UTF8.GetString(bytes);
            var badgeDto = BadgeUtilService.ParseBadge(badgeContent);
            return badgeDto;
        }

        private static string? GetRelativePath(string url, IEnumerable<Reference> refs)
        {
            var uri = new Uri(url);
            var afterTreeSegments = string.Join("", uri.Segments[4..]);
            foreach (var reference in refs)
            {
                var branchInRefWithEndingSlash = $"{Regex.Replace(reference.Ref, @"^[^/]+/[^/]+/", "")}/";

                if (!afterTreeSegments.StartsWith(branchInRefWithEndingSlash))
                    continue;

                var path = afterTreeSegments.Replace(branchInRefWithEndingSlash, string.Empty).TrimEnd('/');
                return path;
            }

            return null;
        }

        private static string? GetBranchNameFromUrl(string url, IEnumerable<Reference> refs)
        {
            var uri = new Uri(url);
            var afterTreeSegments = string.Join("", uri.Segments[4..]);
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

        private static string GetLastSegmentFromUrl(string url, IEnumerable<Reference> refs, out string? parentFolderPath)
        {
            var uri = new Uri(url);
            var lastSegment = uri.Segments.Last().TrimEnd('/');
            var parentFolderUrl = uri.GetLeftPart(UriPartial.Authority) +
                                  string.Join("", uri.Segments.Take(uri.Segments.Length - 1));

            parentFolderPath = GetRelativePath(parentFolderUrl, refs);

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