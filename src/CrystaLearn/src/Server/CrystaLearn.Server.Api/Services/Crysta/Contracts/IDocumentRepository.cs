using CrystaLearn.Server.Api.Models.Crysta;

namespace CrystaLearn.Server.Api.Services.Crysta.Contracts;

public interface IDocumentRepository
{
    Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken);
    Task<Document?> GetDocumentByCodeAsync(string programCode, string code, CancellationToken cancellationToken);
}
