using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<CrystaDocument>> GetDocumentsByCrystaUrlAsync(string crystaUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(crystaUrl))
        {
            return new List<CrystaDocument>();
        }

        return await DbContext.CrystaDocument
            .AsNoTracking()
            .Where(d => d.CrystaUrl == crystaUrl && d.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CrystaDocument>> GetDocumentsByProgramCodeAsync(string programCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(programCode))
        {
            return new List<CrystaDocument>();
        }

        return await DbContext.CrystaDocument
            .AsNoTracking()
            .Where(d => d.CrystaProgram != null && d.CrystaProgram.Code == programCode && d.IsActive)
            .OrderBy(d => d.FileName)
            .ToListAsync(cancellationToken);
    }
}
