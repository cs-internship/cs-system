using Pulumi;
using CrystallineSociety.Iac;

public class Program
{
    static Task<int> Main() => Deployment.RunAsync<TdStack>();
}
