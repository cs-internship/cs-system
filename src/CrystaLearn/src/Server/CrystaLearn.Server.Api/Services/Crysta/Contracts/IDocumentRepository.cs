using CrystaLearn.Server.Api.Models.Crysta;

namespace CrystaLearn.Server.Api.Services.Crysta.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetDocumentsAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<Document?> GetDocumentByCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken);
}
