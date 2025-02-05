using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Server.Api.Controllers.Crysta;

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
        var dtos = list.Select(x => x.Map()).ToList();
        dtos.ForEach(o => o.Content = null);
        return dtos;
    }

    [AllowAnonymous]
    [HttpPost("{culture}")]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<DocumentDto?> GetContentByCrystaUrl([FromBody] string crystaUrl, string culture,
        CancellationToken cancellationToken)
    {
        var result = await DocumentRepository.GetDocumentByCrystaUrlAsync(crystaUrl, culture, cancellationToken);
        return result;
    }

   
}
