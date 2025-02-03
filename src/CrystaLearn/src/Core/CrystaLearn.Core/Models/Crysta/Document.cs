using System.Xml.Linq;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

public class Document : Entity
{
    [MaxLength(150)]
    public virtual string Code { get; set; } = default!;
    [MaxLength(200)]
    public virtual string Title { get; set; } = default!;
    [MaxLength(10)] 
    public virtual string Culture { get; set; } = default!;
    public virtual string? Content { get; set; }
    [MaxLength(300)]
    public virtual string? SourceHtmlUrl { get; set; }
    [MaxLength(300)]
    public virtual string? SourceContentUrl { get; set; }
    [MaxLength(300)]
    public virtual string? CrystaUrl { get; set; }
    [MaxLength(300)]
    public virtual string? Folder { get; set; }
    [MaxLength(300)]
    public virtual string? FileName { get; set; }
    [MaxLength(100)]
    public virtual string? LastHash { get; set; }
    public virtual bool IsActive { get; set; } = true;
    public virtual SyncInfo SyncInfo { get; set; } = new();
    public virtual Guid? CrystaProgramId { get; set; }
    public virtual CrystaProgram? CrystaProgram { get; set; }
}
