using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgan.PoracleWebNet.Data.Entities;

namespace Pgan.PoracleWebNet.Data.Configurations;

public class WebhookDelegateConfiguration : IEntityTypeConfiguration<WebhookDelegateEntity>
{
    public void Configure(EntityTypeBuilder<WebhookDelegateEntity> builder)
    {
        builder.Property(e => e.WebhookId)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasIndex(e => new { e.WebhookId, e.UserId })
            .IsUnique();
    }
}
