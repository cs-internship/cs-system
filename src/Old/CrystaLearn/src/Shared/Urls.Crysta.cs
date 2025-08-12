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
            /// <summary>
            /// 
            /// </summary>
            /// <param name="documentPath">
            /// Sample format: /processes/nervous-system
            /// </param>
            /// <returns></returns>
            public string DocPage(string documentPath) => $"{DocsPage.TrimEnd('/')}/{documentPath.Trim('/')}";
        }
    }

    
}
