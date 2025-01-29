using AutoMapper;

namespace CrystallineSociety.Server.Controllers;

public partial class AppControllerBase : ControllerBase
{
    [AutoInject] protected AppSettings AppSettings = default!;

    [AutoInject] protected AppDbContext DbContext = default!;
    
    [AutoInject] protected IMapper Mapper = default!;

    [AutoInject] protected IStringLocalizer<AppStrings> Localizer = default!;
}
