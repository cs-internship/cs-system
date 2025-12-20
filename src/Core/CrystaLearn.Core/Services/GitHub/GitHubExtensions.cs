using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Shared;
using Markdig;

namespace CrystaLearn.Core.Services.GitHub;
public static class GitHubExtensions
{
    public static Models.Crysta.CrystaDocument CreateDocument(this GitHubItem item, CrystaProgram program)
    {
        string culture = "";
        string programDocUrl = program.DocumentUrl ?? throw new Exception($"Program with code '{program.Code}' has no document url.");

        var culturePosition = item.FileNameWithoutExtension.LastIndexOf("--", StringComparison.Ordinal);
        if (culturePosition == -1)
        {
            culturePosition = item.FileNameWithoutExtension.Length;
        }
        else
        {
            culture = item.FileNameWithoutExtension.Substring(culturePosition + 2).Trim();
        }

        if (string.IsNullOrWhiteSpace(culture))
        {
            if (item.FileNameWithoutExtension.ToLower().Contains("farsi"))
            {
                culture = "fa";
            }
            else
            {
                culture = "en";
            }
        }

        culture = culture.Replace("farsi", "fa");


        var title = item.FileNameWithoutExtension.Substring(0, culturePosition).Trim();
        var code = title
                   .ToLower()
                   .Replace(" ", "-")
                   .Replace("--", "-")
                   .Replace("--", "-");

        var docUrlInfo = GitHubUtil.GetFolderUrlInfo(programDocUrl);

        var relativePath = item.RelativeFolderPath.Substring(docUrlInfo.Path.Length);
        var folderPathPart = relativePath + (!string.IsNullOrEmpty(relativePath) ? "/" : "");
        var crystaUrl = Urls.Crysta.Program(program.Code).DocPage($"{relativePath}/{code}");

        var folderPath = relativePath;

        var doc = new Models.Crysta.CrystaDocument
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = title,
            Culture = culture,
            Content = null,
            SourceHtmlUrl = item.HtmlUrl,
            SourceContentUrl = item.GitHubUrl,
            CrystaUrl = crystaUrl,
            Folder = folderPath,
            FileName = item.FileName,
            LastHash = item.Sha,
            IsActive = true,
            CrystaProgram = program,
            FileExtension = item.FileExtension,
            FileNameWithoutExtension = item.FileNameWithoutExtension,
            DocumentType = item.FileExtension.GetDocumentType(),
            SyncInfo = new SyncInfo()
            {
                SyncStatus = SyncStatus.Success,
                ContentHash = item.Sha,
                SyncStartDateTime = DateTimeOffset.Now,
            }
        };
        return doc;
    }

    public static string? GetHtmlContent(this Models.Crysta.CrystaDocument doc)
    {
        var content = doc.Content;
        if (content == null)
        {
            return null;
        }

        string html = doc.DocumentType switch
        {
            DocumentType.Markdown => Markdown.ToHtml(content),
            DocumentType.Html => content,
            _ => content
        };

        return html;
    }

    public static DocumentType GetDocumentType(this string fileExtension)
    {
        return fileExtension switch
        {
            ".md" => DocumentType.Markdown,
            ".html" => DocumentType.Html,
            ".pdf" => DocumentType.Pdf,
            ".pptx" => DocumentType.PowerPoint,
            ".ppt" => DocumentType.PowerPoint,
            _ => DocumentType.None
        };
    }
}
