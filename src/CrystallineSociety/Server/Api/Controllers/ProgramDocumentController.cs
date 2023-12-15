using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Api.Services.Implementations;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Server.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class ProgramDocumentController : AppControllerBase
{
    [AutoInject]
    public IProgramDocumentService ProgramDocumentService { get; set; } = default!;

    [HttpGet]
    public async Task<List<ProgramDocumentDto>> GetProgramDocumentsAsync(CancellationToken cancellationToken)
    {
        return Mapper.Map<List<ProgramDocumentDto>>( await ProgramDocumentService.GetAllProgramDocumentsAsync(cancellationToken));
    }

    [HttpPost]
    public async Task CreateProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken)
    {
       await ProgramDocumentService.AddProgramDocumentAsync(document,cancellationToken);
    }

    [HttpPost]
    public async Task SyncOrganizationProgramDocumentsAsync(OrganizationDto organization, CancellationToken cancellationToken)
    {
        await ProgramDocumentService.SyncProgramDocumentsAsync(organization.Code, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateProgramDocumentAsync(ProgramDocument document,CancellationToken cancellationToken)
    {
        await ProgramDocumentService.UpdateProgramDocumentAsync(document, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteProgramDocumentAsync(Guid id, CancellationToken cancellationToken)
    {
        await ProgramDocumentService.DeleteProgramDocumentAsync(id,cancellationToken);
    }
}
