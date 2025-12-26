using CrystaLearn.Core.Data;
using CrystaLearn.Core.Extensions;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services.Sync;

public partial class GitHubSyncService : IGitHubSyncService
{
    [AutoInject] private IDocumentRepository DocumentRepository { get; set; } = default!;
    [AutoInject] private AppDbContext DbContext { get; set; } = default!;
    [AutoInject] private ICrystaProgramSyncModuleService CrystaProgramSyncModuleService { get; set; } = default!;
    [AutoInject] private ICrystaDocumentService CrystaDocumentService { get; set; } = default!;

    public async Task<SyncResult> SyncAsync(CrystaProgramSyncModule module, CancellationToken cancellationToken = default)
    {
        if (module.ModuleType != SyncModuleType.GitHubDocument)
        {
            throw new InvalidOperationException("Invalid module type");
        }

        var program = module.CrystaProgram ?? throw new ArgumentNullException(nameof(module.CrystaProgram));
        var programCode = program.Code ?? throw new ArgumentNullException(nameof(program.Code));

        module.SyncInfo.SyncStartDateTime = DateTimeOffset.Now;

        // Fetch documents from GitHub using DocumentRepositoryInMemory
        var githubDocuments = await DocumentRepository.GetDocumentsAsync(programCode, cancellationToken);

        // Sync documents to database
        var syncResult = await SyncDocumentsAsync(githubDocuments, program.Id, cancellationToken);

        // Update module sync info
        module.SyncInfo.LastSyncDateTime = DateTimeOffset.Now;
        module.SyncInfo.SyncEndDateTime = DateTimeOffset.Now;
        module.SyncInfo.SyncStatus = SyncStatus.Success;
        module.UpdatedAt = DateTimeOffset.Now;

        // Persist module sync info
        await CrystaProgramSyncModuleService.UpdateSyncModuleAsync(module);

        return syncResult;
    }

    private async Task<SyncResult> SyncDocumentsAsync(List<CrystaDocument> githubDocuments, Guid programId, CancellationToken cancellationToken)
    {
        var result = new SyncResult { AddCount = 0, UpdateCount = 0, SameCount = 0, DeleteCount = 0 };

        // Get existing documents from database for this program
        var existingDocuments = await DbContext.CrystaDocument
            .Where(d => d.CrystaProgramId == programId)
            .ToListAsync(cancellationToken);

        // Create a map of existing documents by their unique identifier (combination of code and culture)
        
        var existingDocMap = existingDocuments.ToDictionary(d => d.SyncInfo.SyncId, d => d);

        // Track which documents from GitHub we've processed
        var processedKeys = new HashSet<string>();

        // Collect documents to add / update / delete and then save in batch
        var newDocuments = new List<CrystaDocument>();
        var updatedDocuments = new List<CrystaDocument>();
        var deletedDocuments = new List<CrystaDocument>();

        foreach (var githubDoc in githubDocuments)
        {
            var key = githubDoc.SyncInfo.SyncId;
            processedKeys.Add(key);

            if (existingDocMap.TryGetValue(key, out var existingDoc))
            {
                // Document exists - check if it needs updating
                if (existingDoc.LastHash != githubDoc.LastHash)
                {
                    // Document has changed - update it
                    existingDoc.Title = githubDoc.Title;
                    existingDoc.Content = githubDoc.Content;
                    existingDoc.SourceHtmlUrl = githubDoc.SourceHtmlUrl;
                    existingDoc.SourceContentUrl = githubDoc.SourceContentUrl;
                    existingDoc.CrystaUrl = githubDoc.CrystaUrl;
                    existingDoc.Folder = githubDoc.Folder;
                    existingDoc.FileName = githubDoc.FileName;
                    existingDoc.FileExtension = githubDoc.FileExtension;
                    existingDoc.FileNameWithoutExtension = githubDoc.FileNameWithoutExtension;
                    existingDoc.DocumentType = githubDoc.DocumentType;
                    existingDoc.LastHash = githubDoc.LastHash;
                    existingDoc.IsActive = true;
                    existingDoc.SyncInfo.LastSyncDateTime = DateTimeOffset.Now;
                    existingDoc.SyncInfo.ContentHash = githubDoc.LastHash;
                    existingDoc.SyncInfo.SyncStatus = SyncStatus.Success;
                    existingDoc.UpdatedAt = DateTimeOffset.Now;

                    result.UpdateCount++;
                    // Keep track to update in batch
                    updatedDocuments.Add(existingDoc);
                }
                else
                {
                    // Document unchanged
                    result.SameCount++;
                }
            }
            else
            {
                // New document - prepare to add it in batch
                githubDoc.CrystaProgramId = programId;
                githubDoc.CreatedAt = DateTimeOffset.Now;
                githubDoc.UpdatedAt = DateTimeOffset.Now;
                githubDoc.SyncInfo.LastSyncDateTime = DateTimeOffset.Now;
                githubDoc.SyncInfo.SyncStatus = SyncStatus.Success;
                githubDoc.SyncInfo.ContentHash = githubDoc.LastHash;
                newDocuments.Add(githubDoc);
                result.AddCount++;
            }
        }

        // Mark documents that are no longer in GitHub as inactive
        foreach (var existingDoc in existingDocuments)
        {
            var key = existingDoc.SyncInfo.SyncId;
            if (!processedKeys.Contains(key) && existingDoc.IsActive)
            {
                existingDoc.IsActive = false;
                existingDoc.UpdatedAt = DateTimeOffset.Now;
                result.DeleteCount++;
                deletedDocuments.Add(existingDoc);
            }
        }

        // Persist changes via the document service (batch save)
        await CrystaDocumentService.SaveDocumentsAsync(
            newDocuments,
            updatedDocuments,
            deletedDocuments,
            cancellationToken);

        return result;
    }
}
