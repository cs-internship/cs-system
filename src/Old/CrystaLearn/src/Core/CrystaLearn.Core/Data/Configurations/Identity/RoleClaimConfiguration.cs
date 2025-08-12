using CrystaLearn.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrystaLearn.Core.Data.Configurations.Identity;

public partial class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasIndex(userClaim => new { userClaim.RoleId, userClaim.ClaimType });
    }
}
