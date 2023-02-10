using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Utils;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public class BadgeService : IBadgeService
    {
        public BadgeDto? Parse(string specJson)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            var badge = JsonSerializer.Deserialize<BadgeDto>(specJson, options);
            return badge;
        }
    }
}
