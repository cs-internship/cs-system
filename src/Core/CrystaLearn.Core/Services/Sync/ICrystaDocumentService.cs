using CrystaLearn.Core.Models.Crysta;
namespace CrystaLearn.Core.Services.Sync;

public interface ICrystaDocumentService
{
    Task SaveDocumentsAsync(
        IEnumerable<CrystaDocument> newDocuments,
        IEnumerable<CrystaDocument> updatedDocuments,
        IEnumerable<CrystaDocument> deletedDocuments,
        CancellationToken cancellationToken = default);

    Task<List<CrystaLearn.Core.Models.Crysta.CrystaDocument>> GetDocumentsByCrystaUrlAsync(string crystaUrl, CancellationToken cancellationToken = default);

    Task<List<CrystaLearn.Core.Models.Crysta.CrystaDocument>> GetDocumentsByProgramCodeAsync(string programCode, CancellationToken cancellationToken = default);
}
