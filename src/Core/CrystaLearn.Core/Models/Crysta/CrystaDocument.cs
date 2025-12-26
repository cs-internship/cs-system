using System.Xml.Linq;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Crysta;

[Table("CrystaDocument", Schema = "CrystaLearn")]
public class CrystaDocument : Entity
{
    public virtual string Code { get; set; } = default!;

    [MaxLength(150)]
    public virtual string DisplayCode { get; set; } = default!;

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
    [MaxLength(50)]
    public string? FileExtension { get; set; }
    [MaxLength(300)]
    public string? FileNameWithoutExtension { get; set; }
    public DocumentType DocumentType { get; set; } = DocumentType.None;
    public override string ToString()
    {
        return $"{FileName}";
    }
}
