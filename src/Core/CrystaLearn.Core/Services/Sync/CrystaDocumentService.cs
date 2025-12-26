using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Data;

namespace CrystaLearn.Core.Services.Sync;

public partial class CrystaDocumentService : ICrystaDocumentService
{
    [AutoInject] private AppDbContext DbContext { get; set; } = default!;

    public async Task SaveDocumentsAsync(
        IEnumerable<CrystaDocument> newDocuments,
        IEnumerable<CrystaDocument> updatedDocuments,
        IEnumerable<CrystaDocument> deletedDocuments,
        CancellationToken cancellationToken = default)
    {
        if (newDocuments != null && newDocuments.Any())
        {
            await DbContext.CrystaDocument.AddRangeAsync(newDocuments);
        }

        if (updatedDocuments != null && updatedDocuments.Any())
        {
            // Updated documents may already be tracked; ensure they are marked modified
            DbContext.CrystaDocument.UpdateRange(updatedDocuments);
        }

        if (deletedDocuments != null && deletedDocuments.Any())
        {
            foreach (var d in deletedDocuments)
            {
                d.IsActive = false;
                d.UpdatedAt = DateTimeOffset.Now;
            }
            DbContext.CrystaDocument.UpdateRange(deletedDocuments);
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
