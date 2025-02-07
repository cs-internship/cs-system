using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class CrystaTaskUpdate : Entity
{
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;
    public User? User { get; set; }
    public SyncInfo? SyncInfo { get; set; }
    public CrystaProgram? CrystaProgram { get; set; }
}
