using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Shared.Components;
public partial class ProgramDocumentContent
{
    [Parameter] public ProgramDocumentDto? ProgramDocument { get; set; }
}
