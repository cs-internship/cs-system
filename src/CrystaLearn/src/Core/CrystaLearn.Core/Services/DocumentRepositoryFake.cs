using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class DocumentRepositoryFake : IDocumentRepository
{
    public async Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);

        var csInternshipProgram = CrystaProgramRepositoryFake.FakePrograms.FirstOrDefault(p => p.Code == "cs-internship");

        return
            [
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview",
                    Title = "CS Internship Overview",
                    Culture = "en",
                    Content =
                        """
                        <p>
                        Hello this is a <b>document</b>.
                        </p>
                        """,
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CS%20Internship%20Overview%20--farsi-ir.md",
                    CrystaUrl = "/cs-internship-overview",
                    Folder = "interns/",
                    FileName = "CS Internship Overview --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe",
                        SyncStartDateTime = DateTimeOffset.Now,
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview",
                    Title = "CS Internship Overview",
                    Culture = "fa",
                    Content =
                        """
                        <p>
                        این یک مستند فارسی است</b>.
                        </p>
                        """,
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CS%20Internship%20Overview%20--farsi-ir.md",
                    CrystaUrl = "/cs-internship-overview",
                    Folder = "interns/",
                    FileName = "CS Internship Overview --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe",
                        SyncStartDateTime = DateTimeOffset.Now,
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview-other",
                    Title = "Other CS Internship Overview",
                    Culture = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CS%20Internship%20Overview%20--farsi-ir.md",
                    CrystaUrl = "/cs-internship-overview",
                    Folder = "interns/",
                    FileName = "CS Internship Overview --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe",
                        SyncStartDateTime = DateTimeOffset.Now,
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "interview-planning-process",
                    Title = "Interview Planning Process",
                    Culture = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CSI%20-%20Interview%20Planning%20Process%20--farsi-ir.md",
                    CrystaUrl = "/interview-planning-process",
                    Folder = "/mentors",
                    FileName = "CSI - Interview Planning Process --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe",
                        SyncStartDateTime = DateTimeOffset.Now,
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "interview-planning-process-other",
                    Title = "Other Interview Planning Process",
                    Culture = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CSI%20-%20Interview%20Planning%20Process%20--farsi-ir.md",
                    CrystaUrl = "/interview-planning-process",
                    Folder = "/mentors",
                    FileName = "CSI - Interview Planning Process --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe",
                        SyncStartDateTime = DateTimeOffset.Now,
                    }
                }
            ];
    }

    public async Task<Document?> GetDocumentByCodeAsync(
        string programCode,
        string docCode,
        string? culture,
        CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        var list = await GetDocumentsAsync(programCode, cancellationToken);
        var languageVariants = list.Where(d => d.Code == docCode).ToList();

        var document = languageVariants.FirstOrDefault(d => culture?.StartsWith(d.Culture) ?? false);
        if (document is not null)
        {
            return document;
        }

        document = languageVariants.FirstOrDefault(d => d.Culture == "en");
        if (document is not null)
        {
            return document;
        }

        document = languageVariants.FirstOrDefault(d => d.Culture == "fa");
        if (document is not null)
        {
            return document;
        }

        return null;
    }
}
