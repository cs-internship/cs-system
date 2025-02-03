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
        var urlInfo = TextUtil.GetGitHubFolderUrlInfo(url);

        var parentContents = await GitHubClient.Repository.Content.GetAllContentsByRef(
            urlInfo.Owner, 
            urlInfo.RepoName, 
            urlInfo.ParentPath, 
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

    public class GitHubItem
    {
        public required string Sha { get; set; }
        public required string FileName { get; set; }
        public required string HtmlUrl { get; set; }
        public required string RelativeFolderPath { get; set; }
        public required string RelativeFilePath { get; set; }
        public required string GitHubUrl { get; set; }
        public required string FileExtension { get; set; }
        public required string FileNameWithoutExtension { get; set; }
    }

    
}
