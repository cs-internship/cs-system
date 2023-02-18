using CrystallineSociety.Server.Api.Models.TodoItem;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.TodoItem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Server.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public partial class BadgeController : AppControllerBase
{
    [AutoInject]
    public BadgeSystemFactory BadgeSystemFactory { get; set; }

    [HttpGet]
    public IEnumerable<BadgeDto> Get(CancellationToken cancellationToken)
    {
        var badgeService = BadgeSystemFactory.Default();
        return badgeService.Badges;
    }
}
