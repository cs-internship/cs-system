using CrystaLearn.Core.Services.GitHub;
using Octokit;

namespace CrystaLearn.Core.Services.Contracts;

public interface IGitHubService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url">
    ///     Sample URL:
    ///     https://github.com/cs-internship/cs-internship-spec/tree/master/processes/documents
    /// </param>
    /// <returns></returns>
    Task<List<GitHubService.GitHubItem>> GetFilesAsync(string url);

    Task<string?> GetFileContentAsync(string url);
}
