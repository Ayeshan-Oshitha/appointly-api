using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorHub.Infrastructure.Persistence.Configurations
{
    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(d => d.Slug)
                .IsUnique();

            builder.HasIndex(d => d.ProvinceId); // Index on ProvinceId for faster lookups

            builder.HasOne(d => d.Province)      // Each District has one Province
                .WithMany(p => p.Districts)      // Province has many Districts
                .HasForeignKey(d => d.ProvinceId) // Foreign key in District table
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
        }
    }
}
