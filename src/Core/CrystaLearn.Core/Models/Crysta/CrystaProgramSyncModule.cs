using System.Text.Json.Nodes;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class CrystaProgramSyncModule : Entity
{
    public Guid CrystaProgramId { get; set; }
    public CrystaProgram CrystaProgram { get; set; } = default!;
    public SyncModuleType ModuleType { get; set; }
    public string? SyncConfig { get; set; }
    public SyncInfo SyncInfo { get; set; } = new();
}

