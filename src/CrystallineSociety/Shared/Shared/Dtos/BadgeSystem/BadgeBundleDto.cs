using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem
{
    public class BadgeBundleDto
    {
        public BadgeBundleDto()
        {
            
        }

        public BadgeBundleDto(List<BadgeDto> badges)
        {
            Badges = badges;
        }

        public List<BadgeDto> Badges { get; set; } = new();
        public List<BadgeSystemValidationDto>? Validations { get; set; }
        
        public bool BadgeExists(string badgeCode)
        {
            return Badges.Any(b => b.Code == badgeCode);
        }
    }
}
