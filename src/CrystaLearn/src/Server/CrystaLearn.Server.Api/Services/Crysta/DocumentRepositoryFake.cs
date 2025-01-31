using CrystaLearn.Server.Api.Models.Crysta;
using CrystaLearn.Server.Api.Services.Crysta.Contracts;

namespace CrystaLearn.Server.Api.Services.Crysta;

public class DocumentRepositoryFake : IDocumentRepository
{
    public async Task<List<Document>> GetDocumentsAsync(string programCode, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        return
            [
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview",
                    Title = "CS Internship Overview",
                    Language = "fa",
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
                    CrystaProgramId = Guid.NewGuid(),
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe"
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview-other",
                    Title = "Other CS Internship Overview",
                    Language = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CS%20Internship%20Overview%20--farsi-ir.md",
                    CrystaUrl = "/cs-internship-overview",
                    Folder = "interns/",
                    FileName = "CS Internship Overview --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgramId = Guid.NewGuid(),
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe"
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "interview-planning-process",
                    Title = "Interview Planning Process",
                    Language = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CSI%20-%20Interview%20Planning%20Process%20--farsi-ir.md",
                    CrystaUrl = "/interview-planning-process",
                    Folder = "/mentors",
                    FileName = "CSI - Interview Planning Process --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgramId = Guid.NewGuid(),
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe"
                    }
                },
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "interview-planning-process-other",
                    Title = "Other Interview Planning Process",
                    Language = "fa",
                    Content = "content",
                    SourceUrl = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents/CSI%20-%20Interview%20Planning%20Process%20--farsi-ir.md",
                    CrystaUrl = "/interview-planning-process",
                    Folder = "/mentors",
                    FileName = "CSI - Interview Planning Process --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true,
                    CrystaProgramId = Guid.NewGuid(),
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe"
                    }
                }
            ];
    }

    public async Task<Document?> GetDocumentByCodeAsync(string programCode, string code,
        CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);
        var list = await GetDocumentsAsync(programCode, cancellationToken);
        return list.FirstOrDefault(d => d.Code == code);
    }
}
