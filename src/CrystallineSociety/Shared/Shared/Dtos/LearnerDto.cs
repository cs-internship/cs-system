using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Dtos
{
    public class LearnerDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        /// <summary>
        /// Example: +989112223333
        /// </summary>
        public string? MobileNo { get; set; }
        public string? TelegramUsername { get; set; }
        /// <summary>
        /// Example: mehran@hotmail.com
        /// </summary>
        public string? MicrosoftAccount { get; set; }
        /// <summary>
        /// Example: mehran@gmail.com
        /// </summary>
        public string? GoogleAccount { get; set; }
        /// <summary>
        /// Example: mehran@cscore.io
        /// </summary>
        public string? CrsytallineSocietyAccount { get; set; }
        /// <summary>
        /// Example: mehran
        /// </summary>
        public string? TwitterAccount { get; set; }
        /// <summary>
        /// Example: linkedin.com/in/mehran
        /// </summary>
        public string? LinkedInProfileUrl { get; set; }
        public string TagStr { get; set; } = String.Empty;
        public string? BadgesStr { get; set; }
        public string[] GetBadges() => BadgesStr?.Split(' ') ?? new string[]{};
    }
}
