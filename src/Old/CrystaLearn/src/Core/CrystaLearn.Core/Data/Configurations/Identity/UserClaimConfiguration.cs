using CrystaLearn.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrystaLearn.Core.Data.Configurations.Identity;

public partial class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasIndex(userClaim => new { userClaim.UserId, userClaim.ClaimType });
    }
}
