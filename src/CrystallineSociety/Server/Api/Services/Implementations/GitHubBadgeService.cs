using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Json.Converters;
using CrystallineSociety.Shared.Utils;

using Markdig;
using Markdig.Syntax;

using Octokit;

namespace CrystallineSociety.Server.Api.Services.Implementations
{
    public partial class GitHubBadgeService : IGitHubBadgeService
    {
        [AutoInject] public IBadgeUtilService BadgeUtilService { get; set; } = default!;

        [AutoInject] public IProgramDocumentUtilService ProgramDocumentUtilService { get; set; } = default!;

        [AutoInject] public GitHubClient GitHubClient { get; set; } = default!;

        /// <summary>
        /// Asynchronously retrieves <see cref="BadgeDto"/> objects from badge files located at the GitHub URL provided.
        /// This method performs a recursive search for files and selects those whose filename ends with `-badge.json`.
        /// </summary>
        /// <param name="folderUrl">GitHub folder URL containing badge files.</param>
        /// <returns>A task that represents a list of parsed badge files.</returns>
        /// <exception cref="Exception">When the RepoId of light badge is null.</exception>
        /// <exception cref="Exception">When the Sha of light badge is null.</exception>
        public async Task<List<BadgeDto>> GetBadgesAsync(string folderUrl)
        {
            var lightBadges = await GetLightBadgesAsync(folderUrl);

            var badges = new List<BadgeDto>();

            foreach (var lightBadge in lightBadges)
            {
                if (lightBadge.Url is null)
                    continue;

                //todo Replace Exception with NullReferenceException or another relevant one 
                var badgeDto = await GetBadgeAsync(
                    lightBadge.RepoId ?? throw new Exception("RepoId of light badge is null."),
                    lightBadge.Sha ?? throw new Exception("Sha of light badge is null.")
                );

                badgeDto.Url = lightBadge.Url;

                badges.Add(badgeDto);
            }

            return badges;
        }

        /// <summary>
        /// Asynchronously loads and parses a lightweight version of badges specifications from a GitHub URL pointing to a folder recursively.
        /// </summary>
        /// <param name="folderUrl">The GitHub URL pointing to a folder containing badge file(s). All badge files will load recursively. Badge filename must ends with `-badge.json`.</param>
        /// <returns>A task that represents a list of lightweight version of <see cref="BadgeDto"/>s.</returns>
        public async Task<List<BadgeDto>> GetLightBadgesAsync(string folderUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(folderUrl);
            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);

            var lastSegment = GetLastSegmentFromUrl(folderUrl, refs, out var parentFolderPath);
            var repositoryId = repo.Id;

            var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId, parentFolderPath);
            var folderSha = folderContents?.First(f => f.Name == lastSegment).Sha;
            var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, folderSha);

            return allContents.Tree
                .Where(t => t.Type == TreeType.Blob)
                .Select(t => new BadgeDto { RepoId = repositoryId, Sha = t.Sha, Url = t.Url })
                .ToList();
        }

        /// <summary>
        /// Asynchronously loads a badge specification from the given <paramref name="badgeUrl"/> and parses it.
        /// </summary>
        /// <param name="badgeUrl">The badge file GitHub URL. Badge filename must ends with `-badge.json`.</param>
        /// <returns>A task that represents the parsed badge object.</returns>
        /// <exception cref="ResourceNotFoundException">When unable to locate the GitHub repo branchName.</exception>
        /// <exception cref="FileNotFoundException">When the given <paramref name="badgeUrl"/> is not a valid badge file URL.</exception>
        /// <exception cref="FormatException">When the loaded badge file has an incorrect format and cannot be parsed.</exception>
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

        /// <summary>
        /// Asynchronously loads and parses a badge specification from a badge file on GitHub identified by the <paramref name="repositoryId"/> and <paramref name="sha"/> parameters.
        /// </summary>
        /// <param name="repositoryId">The Id of the repository on GtiHub.</param>
        /// <param name="sha">The SHA Id of the file in the repository on GtiHub.</param>
        /// <returns>A task that represents the parsed badge object.</returns>
        public async Task<BadgeDto> GetBadgeAsync(long repositoryId, string sha)
        {
            var badgeBlob = await GitHubClient.Git.Blob.Get(repositoryId, sha);

            var bytes = Convert.FromBase64String(badgeBlob.Content);
            var badgeContent = Encoding.UTF8.GetString(bytes);
            var badgeDto = BadgeUtilService.ParseBadge(badgeContent);
            return badgeDto;
        }

        public async Task<List<ProgramDocumentDto>> GetProgramDocumentsAsync(string folderUrl)
        {
            var lightProgramDocuments = await GetLightProgramDocumentsAsync(folderUrl);

            var programDocuments = new List<ProgramDocumentDto>();

            foreach (var lightProgramDocument in lightProgramDocuments)
            {
                if (lightProgramDocument.Url is null)
                    continue;

                var programDocumentDto = await GetProgramDocumentAsync(lightProgramDocument.RepoId ?? throw new NullReferenceException("RepoId of light parsedProgramDocument is null.")
                    , lightProgramDocument.Sha ?? throw new NullReferenceException("Sha of light parsedProgramDocument is null."));

                programDocumentDto.Url = lightProgramDocument.Url;

                programDocuments.Add(programDocumentDto);
            }

            return programDocuments;
        }

        public async Task<List<ProgramDocumentDto>> GetLightProgramDocumentsAsync(string folderUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(folderUrl);
            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);

            var lastSegment = GetLastSegmentFromUrl(folderUrl, refs, out var parentFolderPath);
            var repositoryId = repo.Id;

            var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId, parentFolderPath);
            var folderSha = folderContents?.First(f => f.Name == lastSegment).Sha;
            var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, folderSha);

            return allContents.Tree
                .Where(t => t.Type == TreeType.Blob)
                .Select(t => new ProgramDocumentDto { RepoId = repositoryId, Sha = t.Sha, Url = t.Url })
                .ToList();
        }

        public async Task<ProgramDocumentDto> GetProgramDocumentAsync(string programDocumentUrl)
        {
            var (orgName, repoName) = GetRepoAndOrgNameFromUrl(programDocumentUrl);

            var repo = await GitHubClient.Repository.Get(orgName, repoName);
            var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);
            var branchName = GetBranchNameFromUrl(programDocumentUrl, refs) ??
                             throw new ResourceNotFoundException($"Unable to locate branchName: {programDocumentUrl}");

            var branchRef = refs.First(r => r.Ref.Contains($"refs/heads/{branchName}"));
            var programDocumentPath = GetRelativePath(programDocumentUrl.EndsWith(".md")
                ? programDocumentUrl
                : throw new FileNotFoundException($"ProgramDocument file not found in: {programDocumentUrl}"), refs);

            var programDocumentContentByte =
                await GitHubClient.Repository.Content.GetRawContentByRef(orgName, repoName, programDocumentPath, branchRef.Ref);

            var programDocumentContent = Encoding.UTF8.GetString(programDocumentContentByte);

            try
            {
                var parsedProgramDocument = ProgramDocumentUtilService.ParseProgramDocument(programDocumentContent);
                return parsedProgramDocument;
            }
            catch (Exception exception)
            {
                throw new FormatException($"Can not parse parsedProgramDocument with ProgramDocumentUrl: '{programDocumentUrl}'", exception);
            }
        }

        public async Task<ProgramDocumentDto> GetProgramDocumentAsync(long repositoryId, string sha)
        {
            var programDocumentBlob = await GitHubClient.Git.Blob.Get(repositoryId, sha);

            var bytes = Convert.FromBase64String(programDocumentBlob.Content);
            var programDocumentContent = Encoding.UTF8.GetString(bytes);

            var htmlContent = Markdown.ToHtml(programDocumentContent);
            string title = string.Empty;

            if (htmlContent is not null)
            {
                string pattern = @"<h1>(.*?)<\/h1>";
                Match match = Regex.Match(htmlContent, pattern);

                if (match.Success)
                {
                    title = match.Groups[1].Value;
                }
            }
            var tempProgramDocumentDto = new ProgramDocumentDto
            {
                Code = title,
                Title = title,
                HtmlContent = htmlContent,
                RepoId = repositoryId,
                Sha = sha,
            };

            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }};

            var programDocumentDto = ProgramDocumentUtilService.ParseProgramDocument(JsonSerializer.Serialize(tempProgramDocumentDto, option));

            return programDocumentDto;
        }

        /// <summary>
        /// The relative path is created by eliminating the URL segments from the left up to the branch name. Branch name is exclude.
        /// </summary>
        /// <param name="url">A GitHub URL pointing to a file/folder.</param>
        /// <param name="refs">Octokit references to the GitHub repo.</param>
        /// <returns>The relative path.</returns>
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

        /// <summary>
        /// Get the branch name from a GitHub URL.
        /// </summary>
        /// <param name="url">A GitHub URL.</param>
        /// <param name="refs">Octokit references to the GitHub repo.</param>
        /// <returns>The branch name.</returns>
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

        /// <summary>
        /// Extract the last segment of a GitHub URL pointing to a folder.
        /// </summary>
        /// <param name="url">A GitHub URL pointing to a folder.</param>
        /// <param name="refs">Octokit references to the GitHub repo.</param>
        /// <param name="parentFolderPath">Extracted parent folder of the given GitHub URL.</param>
        /// <returns>The last segment of a GitHub URL.</returns>
        private static string GetLastSegmentFromUrl(string url, IEnumerable<Reference> refs, out string? parentFolderPath)
        {
            var uri = new Uri(url);
            var lastSegment = uri.Segments.Last().TrimEnd('/');
            var parentFolderUrl = uri.GetLeftPart(UriPartial.Authority) +
                                  string.Join("", uri.Segments.Take(uri.Segments.Length - 1));

            parentFolderPath = GetRelativePath(parentFolderUrl, refs);

            return lastSegment;
        }

        /// <summary>
        /// Retrieves a repository and organization/owner name from GitHub URL.
        /// </summary>
        /// <param name="url">A GitHub URL</param>
        /// <returns>The repository and organization/owner name.</returns>
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