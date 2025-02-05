using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Statistics;
using Microsoft.AspNetCore.Authorization;

namespace CrystaLearn.Shared.Controllers.Crysta;
[Route("api/[controller]/[action]/")]

public interface IDocumentController : IAppController
{
    [HttpGet("{programCode}")]
    Task<List<DocumentDto>> GetDocuments(string programCode, CancellationToken cancellationToken);

    [HttpPost("{culture}")]
    Task<DocumentDto?> GetContentByCrystaUrl(string crystaUrl, string culture,
        CancellationToken cancellationToken);

    
}
