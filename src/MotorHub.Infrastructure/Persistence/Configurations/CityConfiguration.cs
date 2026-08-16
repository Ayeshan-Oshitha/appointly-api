using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorHub.Infrastructure.Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(c => c.Slug)
                .IsUnique();

            builder.HasIndex(c => c.DistrictId); // Index on DistrictId for faster lookups

            builder.HasIndex(c => c.ProvinceId); // Index on ProvinceId for faster lookups

            builder.HasOne(c => c.District)     // Each City has one District
                .WithMany(d => d.Cities)    // District has many Cities
                .HasForeignKey(c => c.DistrictId) // Foreign key in City table
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            builder.HasOne(c => c.Province)     // Each City has one Province
                .WithMany(p => p.Cities)    // Province has many Cities
                .HasForeignKey(c => c.ProvinceId) // Foreign key in City table
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
        }
    }

}
