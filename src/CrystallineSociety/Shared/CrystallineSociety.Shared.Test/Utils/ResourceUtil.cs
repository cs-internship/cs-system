using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Test.Utils
{
    public class ResourceUtil
    {
        public static async Task<string> GetResourceAsync(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(r=>r.Contains(resource));

            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
                throw new InvalidOperationException($"No resource found: '{resource}'");

            using StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
