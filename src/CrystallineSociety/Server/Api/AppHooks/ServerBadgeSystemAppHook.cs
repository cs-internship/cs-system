using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Server.Api.AppHooks
{
    public partial class ServerBadgeSystemAppHook : IAppHook
    {
        [AutoInject]
        public BadgeSystemFactory Factory { get; set; }

        public async Task OnStartup()
        {
            var bundle = new BadgeBundleDto()
            {
                Badges = new List<BadgeDto>
                {
                    new() { Code = "hello" }
                }
            };

            Factory.SetCurrentBadgeSystem(bundle);
        }
    }
}
