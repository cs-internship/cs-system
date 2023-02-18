using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Test.Utils
{
    public class ResourceUtil
    {
        public static async Task<List<string>> GetResourcesAsync(string resource)
        {
            resource = resource.Replace("-", "_");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames().Where(r=>r.Contains(resource));

            var result = new List<string>();

            foreach (var resourceName in resourceNames)
            {
                var stream = assembly.GetManifestResourceStream(resourceName);

                if (stream is null)
                    throw new InvalidOperationException($"No resource found: '{resource}'");

                using StreamReader reader = new StreamReader(stream);
                result.Add(await reader.ReadToEndAsync());
            }

            return result;
        }

        public static async Task<string> LoadSampleBadge(string badge)
        {
            return (await GetResourcesAsync($"{badge}.spec.json")).Single();
        }

        public static Task<List<string>> LoadScenarioBadges(string scenario)
        {
            var resources = GetResourcesAsync(scenario);
            return resources;
        }
    }
}
