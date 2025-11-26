using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

[Table("CrystaTaskRevisions",Schema = "CrystaLearn")]
public class CrystaTaskRevision : CrystaTask
{
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;
    
    [MaxLength(100)]
    public string? RevisionCode { get; set; }
}
