using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class VouncherMapping : IEntityTypeConfiguration<Vouncher>
{
    public void Configure(EntityTypeBuilder<Vouncher> builder)
    {
        builder.ToTable("Vouncher");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired(true)
            .HasColumnType("CHAR")
            .HasMaxLength(8);

        builder.Property(x => x.Title)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.Amount)
            .IsRequired(false)
            .HasColumnType("MONEY");
        
        builder.Property(x => x.IsActive)
            .IsRequired(true)
            .HasColumnType("BIT");
        
        builder.HasIndex(x => x.Number).IsUnique();
    }
}