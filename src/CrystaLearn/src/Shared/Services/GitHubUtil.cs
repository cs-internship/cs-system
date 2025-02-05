using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrystaLearn.Shared.Services;
public static class GitHubUtil
{
    public static GitHubFolderUrlInfo GetFolderUrlInfo(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var owner = segments[1].TrimEnd('/');
        var repo = segments[2].TrimEnd('/');
        var branch = segments[4].TrimEnd('/');
        var path = string.Join("", segments[5..]);
        var parentPath = string.Join("", segments[5..^1]).Trim('/');

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">
    /// Url sample: https://github.com/cs-internship/cs-internship-spec/blob/master/processes/nervous-system.md
    /// </param>
    /// <returns></returns>
    public static GitHubFileUrlInfo GetFileUrlInfo(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var owner = segments[1].TrimEnd('/');
        var repo = segments[2].TrimEnd('/');
        var branch = segments[4].TrimEnd('/');
        var path = string.Join("", segments[5..^1]).Trim('/');
        var fileName = segments[^1].Trim('/');

        var fullPath = $"{path}/{fileName}";

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var fileExtension = Path.GetExtension(fileName);

        var refBranch = $"refs/heads/{branch}";
        var refPath = $"{refBranch}/{path}";

        var result = new GitHubFileUrlInfo
        {
            Owner = owner,
            RepoName = repo,
            Branch = branch,
            RefBranch = refBranch,
            RefPath = refPath,
            Path = path,
            FullPath = fullPath,
            FileName = fileName,
            FileNameWithoutExtension = fileNameWithoutExtension,
            FileExtension = fileExtension
        };

        return result;
    }
    public record GitHubFileUrlInfo()
    {
        public required string Owner { get; init; }
        public required string RepoName { get; init; }
        public required string Branch { get; init; }
        public required string Path { get; init; }
        public required string RefBranch { get; init; }
        public required string RefPath { get; init; }
        public required string FileName { get; init; }
        public required string FileNameWithoutExtension { get; init; }
        public required string FileExtension { get; init; }
        public required string FullPath { get; set; }
    }

    public static (string ProgramCode, string Path) GetCrystaUrlInfo(string url)
    {
        var parts = url.Trim('/').Split('/');
        var program = parts.ElementAtOrDefault(0) ?? throw new ArgumentException("Invalid CrystaUrl");
        var path = string.Join('/', parts[1..]);

        return (program, path);
    }

    public static bool IsRtl(string content)
    {
        return Regex.IsMatch(content[..20], @"\p{IsArabic}|\p{IsHebrew}");
    }
}
