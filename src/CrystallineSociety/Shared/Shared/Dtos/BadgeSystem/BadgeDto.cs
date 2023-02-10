using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos.BadgeSystem
{
    public class BadgeDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public BadgeLevel Level { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }

    public enum BadgeLevel
    {
        Bronze, Silver, Gold
    }
}
