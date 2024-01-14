using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components;

public partial class BadgeContent
{
    [Parameter] public BadgeDto? Badge { get; set; }
}
