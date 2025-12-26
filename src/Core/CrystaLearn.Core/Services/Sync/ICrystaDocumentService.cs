using CrystaLearn.Core.Models.Crysta;
namespace CrystaLearn.Core.Services.Sync;

public interface ICrystaDocumentService
{
    Task SaveDocumentsAsync(
        IEnumerable<CrystaDocument> newDocuments,
        IEnumerable<CrystaDocument> updatedDocuments,
        IEnumerable<CrystaDocument> deletedDocuments,
        CancellationToken cancellationToken = default);
}
