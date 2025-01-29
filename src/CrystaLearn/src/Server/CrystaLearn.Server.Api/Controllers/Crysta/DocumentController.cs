﻿using CrystaLearn.Server.Api.Services;
using CrystaLearn.Shared.Dtos.Statistics;
using CrystaLearn.Shared.Controllers.Statistics;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Server.Api.Services.Crysta.Contracts;

namespace CrystaLearn.Server.Api.Controllers.Statistics;

[ApiController, Route("api/[controller]/[action]")]
public partial class DocumentController : AppControllerBase, IDocumentController
{
    [AutoInject] private IDocumentRepository DocumentRepository { get; set; }

    [AllowAnonymous]
    [HttpGet("{organizationId}")]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<List<DocumentDto>> GetProgramDocuments(Guid organizationId, CancellationToken cancellationToken)
    {
        var list = await DocumentRepository.GetProgramDocumentsAsync(organizationId, cancellationToken);
        return list.Select(x => x.Map()).ToList();
    }
}
