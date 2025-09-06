using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Core.Services.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken);
    Task<DocumentDto?> GetDocumentByCrystaUrlAsync(string crystaUrl, string? culture,
        CancellationToken cancellationToken);

}
