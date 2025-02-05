using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.GitHub;
using Markdig;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrystaLearn.Core.Services;

public partial class DocumentRepositoryInMemory : IDocumentRepository
{
    [AutoInject] private IGitHubService GitHubService { get; set; } = default;
    [AutoInject] private ICrystaProgramRepository CrystaProgramRepository { get; set; } = default;

    private ConcurrentDictionary<string, List<Document>> ProgramDocs { get; set; } = new();

    public async Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken)
    {
        if (ProgramDocs.TryGetValue(programCode, out var list))
        {
            return list;
        }

        var docs = await GetProgramDocsFromGitHubAsync(programCode, CancellationToken.None);

        ProgramDocs.AddOrUpdate(programCode, docs, (key, oldValue) => docs);

        return docs;
    }

    public async Task<Document?> GetDocumentByCrystaUrlAsync(string crystaUrl, string? culture, CancellationToken cancellationToken)
    {
        //var crystaUrlBuilder = new StringBuilder($"/{programCode}/");
        //if (!string.IsNullOrEmpty(crystaUrl) && crystaUrl != "/")
        //    crystaUrlBuilder.Append($"/{crystaUrl}");

        //crystaUrlBuilder.Append($"/{docCode}");

        //var crystaUrl = crystaUrlBuilder.ToString();

        //var url = Urls.Crysta.Program(programCode).DocPage(crystaUrl);
        var programCode = GitHubUtil.GetCrystaUrlInfo(crystaUrl).ProgramCode;
        var docs = await GetDocumentsAsync(programCode, cancellationToken);
        var languageVariants = docs.Where(
            o => o.CrystaUrl == crystaUrl
        ).ToList();

        var document = languageVariants.FirstOrDefault(d => culture?.StartsWith(d.Culture) ?? false);
        document ??= languageVariants.FirstOrDefault(d => d.Culture == "en");
        document ??= languageVariants.FirstOrDefault(d => d.Culture == "fa");

        if (document is null)
        {
            return null;
        }

        await PopulateContentAsync(document);
        
        return document;
    }

    public async Task<string?> GetDocumentContentByUrlAsync(string programCode, string url, CancellationToken cancellationToken)
    {
        var docs = await GetDocumentsAsync(programCode, cancellationToken);
        var doc = docs.First(o => o.SourceHtmlUrl == url);

        await PopulateContentAsync(doc);
        return doc.Content;
    }

    private async Task PopulateContentAsync(Document document)
    {
        if (string.IsNullOrWhiteSpace(document.SourceHtmlUrl))
        {
            throw new Exception("Document has no source html url.");
        }
        
        document.Content ??= await GitHubService.GetFileContentAsync(document.SourceHtmlUrl);
        document.Content = document.GetHtmlContent();
    }

    private async Task<List<Document>> GetProgramDocsFromGitHubAsync(string programCode, CancellationToken cancellationToken)
    {
        var program = await CrystaProgramRepository.GetCrystaProgramByCodeAsync(programCode, cancellationToken);
        if (program == null)
        {
            throw new Exception($"Program with code '{programCode}' not found.");
        }

        string programDocumentUrl = program.DocumentUrl ?? throw new Exception($"Program with code '{programCode}' has no document url.");

        var list = await GitHubService.GetFilesAsync(programDocumentUrl);

        var result = new List<Document>();

        foreach (var item in list)
        {
            var doc = item.CreateDocument(program);
            result.Add(doc);
        }

        return result;

        
    }
}
