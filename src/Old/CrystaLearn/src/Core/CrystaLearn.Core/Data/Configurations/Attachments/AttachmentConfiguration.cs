using CrystaLearn.Core.Models.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrystaLearn.Core.Data.Configurations.Attachments;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(attachment => new { attachment.Id, attachment.Kind });
    }
}
