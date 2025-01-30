using CrystaLearn.Server.Api.Models.Crysta;
using CrystaLearn.Server.Api.Services.Crysta.Contracts;

namespace CrystaLearn.Server.Api.Services.Crysta;

public partial class DocumentRepositoryFake : IDocumentRepository
{
    public async Task<List<Document>> GetDocumentsAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        var csInternshipProgram = CrystaProgramRepositoryFake.FakePrograms.FirstOrDefault(p => p.Code == "cs-internship");

        return
            [
                new Document
                {
                    Id = Guid.NewGuid(),
                    Code = "cs-internship-overview",
                    Title = "CS Internship Overview",
                    Language = "fa",
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
                    CrystaProgram = csInternshipProgram,
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
                    CrystaProgram = csInternshipProgram,
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
                    CrystaProgram = csInternshipProgram,
                    SyncInfo = new SyncInfo()
                    {
                        SyncStatus = SyncStatus.Success,
                        SyncHash = "0xa5b6fe"
                    }
                }
            ];
    }

    public async Task<Document?> GetDocumentByCodeAsync(Guid organizationId, string code, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);
        return new Document
        {
            Id = Guid.NewGuid(),
            Code = "cs-internship-overview",
            Title = "CS Internship Overview",
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
        };
    }
}
