namespace CrystaLearn.Core.Services.GitHub;

public class GitHubItem
{
    public required string Sha { get; set; }
    public required string FileName { get; set; }
    public required string HtmlUrl { get; set; }
    public required string RelativeFolderPath { get; set; }
    public required string RelativeFilePath { get; set; }
    public required string GitHubUrl { get; set; }
    public required string FileExtension { get; set; }
    public required string FileNameWithoutExtension { get; set; }
}
