using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class DocumentRepositoryDirectGitHub : IDocumentRepository
{
    [AutoInject] private IGitHubService GitHubService { get; set; } = default;
    [AutoInject] private ICrystaProgramRepository CrystaProgramRepository { get; set; } = default;

    public async Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken)
    {
        var program = await CrystaProgramRepository.GetCrystaProgramByCodeAsync(programCode, cancellationToken);
        if (program == null)
        {
            throw new Exception($"Program with code '{programCode}' not found.");
        }
        
        string documentUrl = program.DocumentUrl ?? throw new Exception($"Program with code '{programCode}' has no document url.");

        var list = await GitHubService.GetFilesAsync(documentUrl);

        var result = new List<Document>();

        var culture = "";

        foreach(var item in list)
        {
            var culturePosition = item.FileNameWithoutExtension.LastIndexOf("--", StringComparison.Ordinal);
            if (culturePosition == -1)
            {
                culturePosition = item.FileNameWithoutExtension.Length;
            }
            else
            {
                culture = item.FileNameWithoutExtension.Substring(culturePosition + 2).Trim();
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                if (item.FileNameWithoutExtension.ToLower().Contains("farsi"))
                {
                    culture = "fa";
                }
                else
                {
                    culture = "en";
                }
            }

            culture = culture.Replace("farsi", "fa");


            var title = item.FileNameWithoutExtension.Substring(0, culturePosition).Trim();
            var code = title
                .ToLower()
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("--", "-");

            var docUrlInfo = TextUtil.GetGitHubFolderUrlInfo(documentUrl);

            var relativePath = item.RelativeFolderPath.Substring(docUrlInfo.Path.Length);
            var folderPathPart = relativePath + (!string.IsNullOrEmpty(relativePath) ? "/" : "");
            var crystaUrl = Urls.Crysta.Program(programCode).DocPage($"{folderPathPart}{code}");

            var folderPath = string.IsNullOrWhiteSpace(relativePath) ? "/" : relativePath;

            var doc = new Document
            {
                Id = Guid.NewGuid(),
                Code = code,
                Title = title,
                Culture = culture,
                Content = null,
                SourceHtmlUrl = item.HtmlUrl,
                SourceContentUrl = item.GitHubUrl,
                CrystaUrl = crystaUrl,
                Folder = folderPath,
                FileName = item.FileName,
                LastHash = item.Sha,
                IsActive = true,
                CrystaProgram = program,
                SyncInfo = new SyncInfo()
                {
                    SyncStatus = SyncStatus.Success,
                    SyncHash = item.Sha,
                    SyncStartDateTime = DateTimeOffset.Now,
                }
            };
            result.Add(doc);
        }

        return result;
    }

    public Task<Document?> GetDocumentByCodeAsync(string programCode, string docCode, string? culture, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
