using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Shared.Services;
public static class TextUtil
{
    public static GitHubFolderUrlInfo GetGitHubFolderUrlInfo(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var owner = segments[1].TrimEnd('/');
        var repo = segments[2].TrimEnd('/');
        var branch = segments[4].TrimEnd('/');
        var path = string.Join("", segments[5..]);
        var parentPath = string.Join("", segments[5..^1]);
        if (string.IsNullOrWhiteSpace(parentPath))
            parentPath = "/";

        var refBranch = $"refs/heads/{branch}";
        var refPath = $"{refBranch}/{path}";

        var result = new GitHubFolderUrlInfo
        {
            Owner = owner,
            RepoName = repo,
            Branch = branch,
            RefBranch = refBranch,
            RefPath = refPath,
            Path = path,
            ParentPath = parentPath,
        };

        return result;
    }
    public record GitHubFolderUrlInfo()
    {
        public required string Owner { get; init; }
        public required string RepoName { get; init; }
        public required string Branch { get; init; }
        public required string Path { get; init; }
        public required string ParentPath { get; init; }
        public required string RefBranch { get; init; }
        public required string RefPath { get; init; }
    }
}
