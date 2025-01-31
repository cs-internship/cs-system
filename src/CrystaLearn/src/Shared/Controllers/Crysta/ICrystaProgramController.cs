using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Shared.Controllers.Crysta;

[Route("api/[controller]/[action]/")]
public interface ICrystaProgramController : IAppController
{
    [HttpGet]
    Task<List<CrystaProgramDto>> GetPrograms(CancellationToken cancellationToken);
}
