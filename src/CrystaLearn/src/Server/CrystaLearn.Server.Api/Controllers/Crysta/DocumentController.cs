using CrystaLearn.Server.Api.Services.Crysta.Contracts;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Server.Api.Controllers.Crysta;

[ApiController, Route("api/[controller]/[action]")]
public partial class DocumentController : AppControllerBase, IDocumentController
{
    [AutoInject] private IDocumentRepository DocumentRepository { get; set; }
    [AutoInject] private CultureInfoManager CultureInfoManager { get; set; } = default!;

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
        var culture = CultureInfoManager.DefaultCulture.Name;

        var result = await DocumentRepository.GetDocumentByCodeAsync(programCode, code, culture, cancellationToken);
        return result?.Map();
    }
}
