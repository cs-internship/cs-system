using CrystaLearn.Server.Api.Models.Crysta;

namespace CrystaLearn.Server.Api.Services.Crysta.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetProgramDocumentsAsync(Guid organizationId, CancellationToken cancellationToken);
}
