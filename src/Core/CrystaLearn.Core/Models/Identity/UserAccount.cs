using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Infra;

namespace CrystaLearn.Core.Models.Identity;
public class UserAccount : Entity
{
    public User User { get; set; } = default!;
    public Guid UserId { get; set; }

    public string AccountName { get; set; } = default!;
    public AccountProviderType AccountProviderType { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public string AccountEmail { get; set; } = default!;
    public bool IsVerified { get; set; } = false;
    public VerificationStatus VerificationStatus { get; set; } = default!;
    public string? VerificationCode { get; set; }
    public DateTimeOffset? VerificationCodeExpireDateTime { get; set; }
}
