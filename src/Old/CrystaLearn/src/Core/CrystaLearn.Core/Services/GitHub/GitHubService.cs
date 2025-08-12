using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
    ///     Sample URL:
    ///     https://github.com/cs-internship/cs-internship-spec/tree/master/processes/documents
    /// </param>
    /// <returns></returns>
    public async Task<List<GitHubItem>> GetFilesAsync(string url)
    {
        var urlInfo = GitHubUtil.GetFolderUrlInfo(url);

        var parentPath = string.IsNullOrWhiteSpace(urlInfo.ParentPath) ? "/" : urlInfo.ParentPath;

        var parentContents = await GitHubClient.Repository.Content.GetAllContentsByRef(
            urlInfo.Owner, 
            urlInfo.RepoName,
            parentPath, 
            urlInfo.Branch);

        var folder = parentContents.First(f => f.Path == urlInfo.Path);

        var treeResponse = await GitHubClient.Git.Tree.GetRecursive(urlInfo.Owner, urlInfo.RepoName, folder.Sha);
        
        var result = treeResponse.Tree
                          .Where(t => t.Type == TreeType.Blob)
                          .Select(t =>
                          {
                              var relativeFolderPath = 
                                Path.Combine(urlInfo.Path, Path.GetDirectoryName(t.Path) ?? "/")
                                .Replace(@"\", @"/");
                              var fileName = Path.GetFileName(t.Path);
                              var fileExtension = Path.GetExtension(t.Path);
                              var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(t.Path);
                              var relativeFilePath = 
                                Path.Combine(relativeFolderPath, fileName)
                                .Replace(@"\", @"/");

                              var htmlUrl = HttpUtility.UrlPathEncode(
                                  $"https://github.com/{urlInfo.Owner}/{urlInfo.RepoName}/blob/{urlInfo.Branch}/{relativeFolderPath}/{fileName}");
                              return new GitHubItem
                              {
                                  Sha = t.Sha,
                                  FileName = fileName,
                                  GitHubUrl = t.Url,
                                  HtmlUrl = htmlUrl,
                                  RelativeFolderPath = relativeFolderPath,
                                  RelativeFilePath = relativeFilePath,
                                  FileExtension = fileExtension,
                                  FileNameWithoutExtension = fileNameWithoutExtension
                              };
                          })
                          .ToList();

        return result;
    }

    public async Task<string?> GetFileContentAsync(string url)
    {
        var info = GitHubUtil.GetFileUrlInfo(url);
        var contents = await GitHubClient.Repository.Content.GetAllContentsByRef(info.Owner, info.RepoName, info.FullPath, info.Branch);
        return contents.FirstOrDefault()?.Content;
    }
}
