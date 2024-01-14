using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Core.Controllers;

[Route("api/[controller]/[action]")]
public interface IProgramDocumentController : IAppController
{
    [HttpGet]
     Task<List<ProgramDocumentDto>> GetProgramDocuments(string organizationCode, CancellationToken cancellationToken = default);

    //[HttpPost]
    //Task CreateProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken);

    [HttpPost]
     Task SyncOrganizationProgramDocuments(OrganizationDto organization, CancellationToken cancellationToken = default);

    //[HttpPut("{id}")]
    // Task UpdateProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken);

    [HttpDelete("{id}")]
     Task DeleteProgramDocument(Guid id, CancellationToken cancellationToken = default);
}
