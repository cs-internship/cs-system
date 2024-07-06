using CrystallineSociety.Shared.Dtos.BadgeSystem;
using Microsoft.AspNetCore.Components;

namespace CrystallineSociety.Client.Core.Components;

    public partial class BadgeRequirement
    {
        [Parameter] public IRequirement? Requirement { get; set; }
    }
