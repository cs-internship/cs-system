using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Contracts;
using Octokit;

namespace CrystaLearn.Core.Services.GitHub;
public partial class GitHubService : IGitHubService
{
    [AutoInject] private GitHubClient GitHubClient { get; set; } = default!;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">
    /// Sample URL:
    /// https://github.com/cs-internship/cs-internship-spec/tree/master/processes/documents
    /// </param>
    /// <returns></returns>
    public async Task<List<TreeItem>> GetFilesAsync(string url)
    {
        var (orgName, repoName) = GetRepoAndOrgNameFromUrl(url);
        var repo = await GitHubClient.Repository.Get(orgName, repoName);
        var refs = await GitHubClient.Git.Reference.GetAll(repo.Id);

        var lastSegment = GetLastSegmentFromUrl(url, refs, out var parentFolderPath);
        var repositoryId = repo.Id;

        var folderContents = await GitHubClient.Repository.Content.GetAllContents(repositoryId, parentFolderPath);
        var folderSha = folderContents?.First(f => f.Name == lastSegment).Sha;
        var allContents = await GitHubClient.Git.Tree.GetRecursive(repositoryId, folderSha);

        return allContents.Tree
                          .Where(t => t.Type == TreeType.Blob && t.Path.EndsWith(".md"))
                          .ToList();
    }

    private (string org, string repo) GetRepoAndOrgNameFromUrl(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var org = segments[1].TrimEnd('/');
        var repo = segments[2].TrimEnd('/');

        return (org, repo);
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
}
