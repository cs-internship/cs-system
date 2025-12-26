using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.GitHub;
using CrystaLearn.Shared.Dtos.Crysta;
using Markdig;
using CrystaLearn.Core.Mappers;

namespace CrystaLearn.Core.Services;

public partial class DocumentRepositoryDirectGitHub : IDocumentRepository
{
    [AutoInject] private IGitHubService GitHubService { get; set; } = default;
    [AutoInject] private ICrystaProgramRepository CrystaProgramRepository { get; set; } = default;
    [AutoInject] private CrystaLearn.Core.Services.Sync.ICrystaDocumentService CrystaDocumentService { get; set; } = default!;

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
            if (!string.IsNullOrWhiteSpace(doc.SourceHtmlUrl))
            {
                doc.Content ??= await GitHubService.GetFileContentAsync(doc.SourceHtmlUrl);
                doc.Content = doc.GetHtmlContent();
                doc.Content = new string(doc.Content?.Where(c => c != '\0').ToArray());
            }
            result.Add(doc);
        }

        return result;
    }

    public async Task<DocumentDto?> GetDocumentByCrystaUrlAsync(string crystaUrl,
        string? culture, CancellationToken cancellationToken)
    {
        var languageVariants = await CrystaDocumentService.GetDocumentsByCrystaUrlAsync(crystaUrl, cancellationToken);

        if (languageVariants == null || !languageVariants.Any())
        {
            return null;
        }

        var document = languageVariants.FirstOrDefault(d => culture?.StartsWith(d.Culture) ?? false);
        document ??= languageVariants.FirstOrDefault(d => d.Culture.StartsWith("en"));
        document ??= languageVariants.FirstOrDefault(d => d.Culture.StartsWith("fa"));

        if (document is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(document.SourceHtmlUrl) && string.IsNullOrEmpty(document.Content))
        {
            document.Content ??= await GitHubService.GetFileContentAsync(document.SourceHtmlUrl);
            document.Content = document.GetHtmlContent();
            document.Content = new string(document.Content?.Where(c => c != '\0').ToArray());
        }

        var dto = document.Map();
        dto.CultureVariants = languageVariants.Select(x => x.Culture).ToArray();

        return dto;
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
