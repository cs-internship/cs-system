using CrystaLearn.Core.Models.Identity;

namespace CrystaLearn.Server.Api.Models.Identity;

public class UserToken : IdentityUserToken<Guid>
{
    public User? User { get; set; }
}
