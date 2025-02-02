using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Server.Api.Controllers.Crysta;

[ApiController, Route("api/[controller]/[action]")]
public partial class CrystaProgramController : AppControllerBase, ICrystaProgramController
{
    [AutoInject] private ICrystaProgramRepository CrystaProgramRepository { get; set; } = default!;

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["*"])]
    public async Task<List<CrystaProgramDto>> GetPrograms(CancellationToken cancellationToken)
    {
        var list = await CrystaProgramRepository.GetCrystaProgramsAsync(cancellationToken);
        var result = list.Select<CrystaProgram, CrystaProgramDto>(x => x.Map()).ToList();
        return result;
    }
}
