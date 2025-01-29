using CrystaLearn.Server.Api.Models.Crysta;
using CrystaLearn.Server.Api.Services.Crysta.Contracts;

namespace CrystaLearn.Server.Api.Services.Crysta;

public class DocumentRepositoryFake : IDocumentRepository
{
    public async Task<List<Document>> GetProgramDocumentsAsync(Guid organizationId, CancellationToken cancellationToken)
    {
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
                    Folder = "/",
                    FileName = "CS Internship Overview --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true
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
                    Folder = "/",
                    FileName = "CSI - Interview Planning Process --farsi-ir.md",
                    LastHash = "0xa5b6fe",
                    IsActive = true
                }
            ];
    }
}
