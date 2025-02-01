namespace CrystaLearn.Shared;

public static partial class Urls
{
    public static class Crysta
    {
        public const string DocsPageRoutePattern = "/{ProgramCode}/docs";
        public static ProgramPage Program(string program) => new(program);

        public class ProgramPage(string programCode)
        {
            public string DocsPage = $"/{programCode}/docs";
            public string DocPage(string documentCode) => $"{DocsPage}/{documentCode}";
        }
    }

    
}
