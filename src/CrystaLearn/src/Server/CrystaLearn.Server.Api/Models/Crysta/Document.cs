using System.Xml.Linq;
using CrystaLearn.Server.Api.Models.Infra;

namespace CrystaLearn.Server.Api.Models.Crysta;

public class Document : Entity
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string? Content { get; set; }
    public string? SourceUrl { get; set; }
    public string? CrystaUrl { get; set; }
    public string? Folder { get; set; }
    public string? FileName { get; set; }
    public string? LastHash { get; set; }
    public bool IsActive { get; set; } = true;
}
