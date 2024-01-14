using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components;

public partial class BadgeValidation
{
    [Parameter] public BadgeBundleDto? BadgeBundle { get; set; }
}
