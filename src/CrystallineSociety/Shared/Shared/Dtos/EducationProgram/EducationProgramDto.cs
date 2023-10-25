using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.EducationProgram;
public class EducationProgramDto
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string BadgeSystemUrl { get; set; } = default!;
    public DateTimeOffset LastSyncDateTime { get; set; }
    public string? LastCommitHash { get; set; }
    public bool IsActive { get; set; }
}
