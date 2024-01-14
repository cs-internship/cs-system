using AutoMapper;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Services.Contracts;
using Octokit;

namespace CrystallineSociety.Server.Services.Implementations;

public partial class ProgramDocumentService : IProgramDocumentService
{
    [AutoInject]
    protected AppDbContext AppDbContext = default!;

    [AutoInject]
    protected IMapper Mapper = default!;

    [AutoInject]
    protected IProgramDocumentUtilService ProgramDocumentUtilService { get; set; } = default!;

    [AutoInject]
    protected IOrganizationService OrganizationService { get; set; } = default!;

    [AutoInject] public GitHubClient GitHubClient { get; set; } = default!;

    [AutoInject] public IGitHubBadgeService GitHubBadgeService { get; set; } = default!;

    public async Task SyncProgramDocumentsAsync(string organizationCode, CancellationToken cancellationToken)
    {
        var document = await GetProgramDocumentsByOrganizationAsync(organizationCode, cancellationToken);
        if (document == null)
        {
            throw new Exception($"Program Document with code {organizationCode} not found");
        }

        AppDbContext.ProgramDocument.RemoveRange(document);

        await SaveChangesAsync(cancellationToken);

        var organization = await OrganizationService.GetOrganizationAsync(organizationCode, cancellationToken);

        if (!string.IsNullOrWhiteSpace(organization?.ProgramDocumentUrl))
        {
            var newDocuments = await GitHubBadgeService.GetProgramDocumentsAsync(organization.ProgramDocumentUrl);

            if (newDocuments == null)
            {
                throw new Exception("Failed to fetch new documents from GitHub");
            }

            var newProgramDocuments = newDocuments.Select(document => new ProgramDocument
            {
                Id = Guid.NewGuid(),
                Code = document.Code ?? throw new Exception("document code is null"),
                Title = document.Title ?? throw new Exception("document title is null"),
                SourceUrl = document.Url,
                HtmlContent = document.HtmlContent,
                Language = document.Language,
                LastHash = document.Sha,
                OrganizationId = organization.Id,
                CreateDateTime = DateTimeOffset.UtcNow,
                LastUpdateDateTime = DateTimeOffset.UtcNow
            }).ToList();

            await AppDbContext.ProgramDocument.AddRangeAsync(newProgramDocuments, cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ProgramDocument>> GetAllProgramDocumentsAsync(string organizationCode, CancellationToken cancellationToken)
    {
        var organization = await OrganizationService.GetOrganizationAsync(organizationCode, cancellationToken);

        if (organization is null)
        {
            throw new Exception($"Organization with code {organizationCode} not found");
        }

        return await AppDbContext.ProgramDocument.Where(p => p.OrganizationId == organization.Id).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsProgramDocumentExistForOrganizationAsync(string folderUrl, CancellationToken cancellationToken)
    {
        return await AppDbContext.ProgramDocument.AnyAsync(doc => doc != null && doc.SourceUrl == folderUrl, cancellationToken);
    }

    public async Task<ProgramDocument?> GetProgramDocumentAsync(Guid id, CancellationToken cancellationToken)
    {
        return await AppDbContext.ProgramDocument.FirstOrDefaultAsync(doc => doc != null && doc.Id == id, cancellationToken);
    }

    public async Task<ProgramDocument?> GetProgramDocumentAsync(string folderUrl, CancellationToken cancellationToken)
    {
        return await AppDbContext.ProgramDocument.FirstOrDefaultAsync(doc => doc != null && doc.SourceUrl == folderUrl, cancellationToken);
    }

    public async Task<List<ProgramDocument>?> GetProgramDocumentsByOrganizationAsync(string organizationCode, CancellationToken cancellationToken)
    {
        var organization = await OrganizationService.GetOrganizationAsync(organizationCode, cancellationToken);

        if (organization is not null)
        {
            return await AppDbContext.ProgramDocument.Where(doc => doc.OrganizationId == organization.Id).ToListAsync(cancellationToken);
        }

        throw new Exception($"Organization with code {organizationCode} not found");
    }

    public async Task AddProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken)
    {
        await AppDbContext.ProgramDocument.AddAsync(document, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProgramDocumentAsync(ProgramDocument document, CancellationToken cancellationToken)
    {
        AppDbContext.ProgramDocument.Update(document);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProgramDocumentAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = await AppDbContext.ProgramDocument.FirstOrDefaultAsync(doc => doc.Id == id, cancellationToken);
        if (document != null)
        {
            AppDbContext.ProgramDocument.Remove(document);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteProgramDocumentAsync(string folderUrl, CancellationToken cancellationToken)
    {
        var document = await AppDbContext.ProgramDocument.FirstOrDefaultAsync(doc => doc.SourceUrl == folderUrl, cancellationToken);
        if (document != null)
        {
            AppDbContext.ProgramDocument.Remove(document);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await AppDbContext.SaveChangesAsync(cancellationToken);
    }

    public void SaveChanges()
    {
        AppDbContext.SaveChanges();
    }
}
