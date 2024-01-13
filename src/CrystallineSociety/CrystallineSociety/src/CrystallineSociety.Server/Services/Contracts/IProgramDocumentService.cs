using CrystallineSociety.Server.Api.Models;

namespace CrystallineSociety.Server.Services.Contracts;

/// <summary>
/// Represents a service for managing program documents.
/// </summary>
public interface IProgramDocumentService
{
    /// <summary>
    /// Synchronizes program documents asynchronously.
    /// </summary>
    /// <param name="organizationCode">The organization code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SyncProgramDocumentsAsync(string organizationCode, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all program documents asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of program documents.</returns>
    Task<List<ProgramDocument>> GetAllProgramDocumentsAsync(string organizationCode, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a program document exists asynchronously.
    /// </summary>
    /// <param name="folderUrl">The program document URL.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the program document exists; otherwise, false.</returns>
    Task<bool> IsProgramDocumentExistForOrganizationAsync(string folderUrl, CancellationToken cancellationToken);

    Task<List<ProgramDocument>?> GetProgramDocumentsByOrganizationAsync(string organizationCode, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a program document asynchronously.
    /// </summary>
    /// <param name="programDocumentCode">The program document code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The program document.</returns>
    Task<ProgramDocument?> GetProgramDocumentAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a program document asynchronously.
    /// </summary>
    /// <param name="folderUrl">The program document URL.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<ProgramDocument?> GetProgramDocumentAsync(string folderUrl, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a program document asynchronously.
    /// </summary>
    /// <param name="document">The program document to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a program document asynchronously.
    /// </summary>
    /// <param name="document">The program document to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a program document asynchronously.
    /// </summary>
    /// <param name="programDocumentCode">The program document code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteProgramDocumentAsync(Guid id, CancellationToken cancellationToken);

    Task DeleteProgramDocumentAsync(string folderUrl, CancellationToken cancellationToken);

    ///// <summary>
    ///// Documents the program document asynchronously.
    ///// </summary>
    ///// <param name="programDocumentCode">The program document code.</param>
    ///// <param name="cancellationToken">The cancellation token.</param>
    //Task DocumentProgramDocumentAsync(Guid id, CancellationToken cancellationToken);

    ///// <summary>
    ///// Documents the program document asynchronously.
    ///// </summary>
    ///// <param name="folderUrl">The program document URL.</param>
    ///// <param name="cancellationToken">The cancellation token.</param>
    //Task DocumentProgramDocumentAsync(string folderUrl, CancellationToken cancellationToken);
}
