using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.GitHub;
using CrystaLearn.Shared.Dtos.Crysta;
using Markdig;

namespace CrystaLearn.Core.Services;

public partial class DocumentRepositoryDirectGitHub : IDocumentRepository
{
    [AutoInject] private IGitHubService GitHubService { get; set; } = default;
    [AutoInject] private ICrystaProgramRepository CrystaProgramRepository { get; set; } = default;

    public async Task<List<CrystaDocument>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken)
    {
        var program = await CrystaProgramRepository.GetCrystaProgramByCodeAsync(programCode, cancellationToken);
        if (program == null)
        {
            throw new Exception($"Program with code '{programCode}' not found.");
        }
        
        string documentUrl = program.DocumentUrl ?? throw new Exception($"Program with code '{programCode}' has no document url.");

        var list = await GitHubService.GetFilesAsync(documentUrl);

        var result = new List<CrystaDocument>();

        var culture = "";

        foreach(var item in list)
        {
            var doc = item.CreateDocument(program);
            result.Add(doc);
        }

        return result;
    }

    public async Task<DocumentDto> GetDocumentByCrystaUrlAsync(string crystaUrl,
        string? culture, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    //public async Task<DocumentDto?> GetDocumentContentByUrlAsync(string programCode, string url,
    //    CancellationToken cancellationToken)
    //{
    //    var content = await GitHubService.GetFileContentAsync(url);

    //    if (content == null)
    //    {
    //        return null;
    //    }
        
    //    var html = Markdown.ToHtml(content);
    //    return html;
    //}
}
