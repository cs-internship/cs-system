using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;


namespace CrystallineSociety.Client.Shared.Components
{
    public partial class BadgeSystem
    {
        [Parameter] public BadgeBundleDto? Bundle { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}
