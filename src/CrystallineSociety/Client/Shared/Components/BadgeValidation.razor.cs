using CrystallineSociety.Shared.Dtos.BadgeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Client.Shared.Components
{
    public partial class BadgeValidation
    {
        [Parameter] public BadgeBundleDto? BadgeBundle { get; set; }
    }
}
