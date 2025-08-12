namespace CrystaLearn.Core.Models.Crysta;

public class CrystaTaskRevision : CrystaTask
{
    public Guid CrystaTaskId { get; set; }
    public CrystaTask CrystaTask { get; set; } = default!;
    public string? RevisionCode { get; set; }

}
