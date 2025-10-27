namespace CrystaLearn.Client.Core;

public class ComponentMetadata
{
    public Type? Type { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = [];
}
