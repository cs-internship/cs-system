using System.Web;
using CrystaLearn.Core.Services.Sync;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Server.Api.Controllers.Crysta;

[ApiController, Route("api/[controller]/[action]")]
public partial class DocumentController : AppControllerBase, IDocumentController
{
    [AutoInject] private ICrystaDocumentService CrystaDocumentService { get; set; }

    [AllowAnonymous]
    [HttpGet("{programCode}")]
    //[ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<List<DocumentDto>> GetDocuments(string programCode, CancellationToken cancellationToken)
    {
        var list = await CrystaDocumentService.GetDocumentsByProgramCodeAsync(programCode, cancellationToken);
        var dtos = list.Select(x => x.Map()).ToList();
        dtos.ForEach(o => o.Content = null);
        return dtos;
    }

    [AllowAnonymous]
    [HttpPost("{culture}")]
    //[ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<DocumentDto?> GetContentByCrystaUrl([FromBody] string crystaUrl, string culture,
        CancellationToken cancellationToken)
    {
        var decoded = HttpUtility.UrlDecode(crystaUrl);
        var docs = await CrystaDocumentService.GetDocumentsByCrystaUrlAsync(decoded, cancellationToken);

        var languageVariants = docs.Where(o => o.CrystaUrl == decoded).ToList();

        var document = languageVariants.FirstOrDefault(d => culture?.StartsWith(d.Culture) ?? false);
        document ??= languageVariants.FirstOrDefault(d => d.Culture.StartsWith("en"));
        document ??= languageVariants.FirstOrDefault(d => d.Culture.StartsWith("fa"));

        if (document is null)
        {
            return null;
        }

        var dto = document.Map();
        dto.CultureVariants = languageVariants.Select(x => x.Culture).ToArray();

        return dto;
    }
}
