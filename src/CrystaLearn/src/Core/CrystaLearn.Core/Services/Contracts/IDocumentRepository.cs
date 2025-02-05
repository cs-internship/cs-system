using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken);
    Task<Document?> GetDocumentByCrystaUrlAsync(string crystaUrl, string? culture,
        CancellationToken cancellationToken);

    Task<string?> GetDocumentContentByUrlAsync(string programCode, string url, CancellationToken cancellationToken);
}
