using CrystaLearn.Core.Models.Crysta;

namespace CrystaLearn.Core.Services.Sync;

public class CrystaDocumentServiceFake : ICrystaDocumentService
{
    private readonly List<CrystaDocument> _store = new();

    public IReadOnlyCollection<CrystaDocument> InMemoryDocuments => _store.AsReadOnly();

    public Task SaveDocumentsAsync(
        IEnumerable<CrystaDocument> newDocuments,
        IEnumerable<CrystaDocument> updatedDocuments,
        IEnumerable<CrystaDocument> deletedDocuments,
        CancellationToken cancellationToken = default)
    {
        if (newDocuments != null)
        {
            foreach (var d in newDocuments)
            {
                var copy = CloneDocument(d);
                _store.Add(copy);
            }
        }

        if (updatedDocuments != null)
        {
            foreach (var d in updatedDocuments)
            {
                var key = GetKey(d);
                var existing = _store.FirstOrDefault(x => GetKey(x) == key);
                if (existing != null)
                {
                    ApplyUpdate(existing, d);
                }
                else
                {
                    _store.Add(CloneDocument(d));
                }
            }
        }

        if (deletedDocuments != null)
        {
            foreach (var d in deletedDocuments)
            {
                var key = GetKey(d);
                var existing = _store.FirstOrDefault(x => GetKey(x) == key);
                if (existing != null)
                {
                    existing.IsActive = false;
                    existing.UpdatedAt = DateTimeOffset.Now;
                }
            }
        }

        return Task.CompletedTask;
    }

    private static string GetKey(CrystaDocument d) => $"{d.Code}_{d.Culture}";

    private static CrystaDocument CloneDocument(CrystaDocument src)
    {
        return new CrystaDocument
        {
            Id = src.Id,
            Code = src.Code,
            Culture = src.Culture,
            Title = src.Title,
            Content = src.Content,
            SourceHtmlUrl = src.SourceHtmlUrl,
            SourceContentUrl = src.SourceContentUrl,
            CrystaUrl = src.CrystaUrl,
            Folder = src.Folder,
            FileName = src.FileName,
            FileExtension = src.FileExtension,
            FileNameWithoutExtension = src.FileNameWithoutExtension,
            DocumentType = src.DocumentType,
            LastHash = src.LastHash,
            IsActive = src.IsActive,
            CreatedAt = src.CreatedAt,
            UpdatedAt = src.UpdatedAt,
            CrystaProgramId = src.CrystaProgramId,
            SyncInfo = src.SyncInfo // assuming SyncInfo is a reference type handled in tests
        };
    }

    private static void ApplyUpdate(CrystaDocument existing, CrystaDocument src)
    {
        existing.Title = src.Title;
        existing.Content = src.Content;
        existing.SourceHtmlUrl = src.SourceHtmlUrl;
        existing.SourceContentUrl = src.SourceContentUrl;
        existing.CrystaUrl = src.CrystaUrl;
        existing.Folder = src.Folder;
        existing.FileName = src.FileName;
        existing.FileExtension = src.FileExtension;
        existing.FileNameWithoutExtension = src.FileNameWithoutExtension;
        existing.DocumentType = src.DocumentType;
        existing.LastHash = src.LastHash;
        existing.IsActive = src.IsActive;
        existing.SyncInfo = src.SyncInfo;
        existing.UpdatedAt = DateTimeOffset.Now;
    }
}
