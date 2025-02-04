using System.Collections.Concurrent;
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

    public async Task<Document?> GetDocumentByCodeAsync(string programCode, string docFullPath, string? culture, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetDocumentContentByUrlAsync(string programCode, string url, CancellationToken cancellationToken)
    {
        var docs = await GetDocumentsAsync(programCode, cancellationToken);
        var doc = docs.First(o => o.SourceHtmlUrl == url);

        doc.Content ??= await GitHubService.GetFileContentAsync(url);
        
        doc.Content = doc.GetHtmlContent();
        
        return doc.Content;
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
