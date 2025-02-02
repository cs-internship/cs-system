using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken);
    Task<Document?> GetDocumentByCodeAsync(string programCode, string docCode, string? culture,
        CancellationToken cancellationToken);
}
