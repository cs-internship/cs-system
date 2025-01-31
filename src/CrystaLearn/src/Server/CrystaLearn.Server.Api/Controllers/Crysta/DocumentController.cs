using CrystaLearn.Server.Api.Services;
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
    [HttpGet("{programCode}")]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<List<DocumentDto>> GetDocuments(string programCode, CancellationToken cancellationToken)
    {
        var list = await DocumentRepository.GetDocumentsAsync(programCode, cancellationToken);
        list.ForEach(o => o.Content = null);
        return list.Select(x => x.Map()).ToList();
    }

    [AllowAnonymous]
    [HttpGet("{programCode}/{code}")]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<DocumentDto?> GetDocumentByCode(string programCode, string code,
        CancellationToken cancellationToken)
    {
        var result = await DocumentRepository.GetDocumentByCodeAsync(programCode, code, cancellationToken);
        return result?.Map();
    }
}
