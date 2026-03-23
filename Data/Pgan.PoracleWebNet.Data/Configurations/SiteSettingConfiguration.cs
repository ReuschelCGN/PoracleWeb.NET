using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pgan.PoracleWebNet.Data.Entities;

namespace Pgan.PoracleWebNet.Data.Configurations;

public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSettingEntity>
{
    public void Configure(EntityTypeBuilder<SiteSettingEntity> builder)
    {
        builder.Property(e => e.Category)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnType("text");

        builder.Property(e => e.ValueType)
            .HasMaxLength(20)
            .HasDefaultValue("string")
            .IsRequired();

        builder.HasIndex(e => e.Key)
            .IsUnique();
    }
}
