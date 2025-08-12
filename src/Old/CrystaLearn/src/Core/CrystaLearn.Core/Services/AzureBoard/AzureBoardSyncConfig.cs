namespace CrystaLearn.Core.Services.AzureBoard;

public class AzureBoardSyncConfig
{
    public string? Organization { get; set; }
    public string? Project { get; set; }
    public string? PersonalAccessToken { get; set; }
    public DateTimeOffset? WorkItemChangedFromDateTime { get; set; }
}
