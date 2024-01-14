using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Models;

namespace CrystallineSociety.Server.Models;

public class ProgramDocument : EntityBase
{
    public ProgramDocument()
    {
    }

    public ProgramDocument(bool initialize) : base(initialize)
    {
    }

    public string? Title { get; set; }
    public required string Code { get; set; }
    public string? Language { get; set; }
    public string? SourceUrl { get; set; }
    public string? HtmlContent { get; set; }
    public string? LastHash { get; set; }
    public DateTimeOffset LastUpdateDateTime { get; set; }
    public DateTimeOffset CreateDateTime { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    [NotMapped]
    public virtual Organization Organization { get; set; }

    public virtual required Guid OrganizationId { get; set; }
}
