using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos;
public class ProgramDocumentDto
{
    public string? Title { get; set; }
    public string? Code { get; set; }
    public string? Language { get; set; }
    public string? HtmlContent { get; set; }

    public string? Sha { get; set; }
    public string? Url { get; set; }
    public long? RepoId { get; set; }
    public override string ToString()
    {
        return Code ?? "";
    }
}
