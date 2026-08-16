using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MotorHub.Infrastructure.Persistence.Configurations
{
    internal class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(a => a.Year)
                .IsRequired();

            builder.Property(a => a.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.EngineCapacity);

            builder.Property(a => a.FuelType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.TransmissionType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.VehicleCondition)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            // Bounded to match the request DTOs; both were unbounded text columns.
            builder.Property(a => a.Address)
                .HasMaxLength(250);

            builder.Property(a => a.RejectedReason)
                .HasMaxLength(500);

            builder.HasOne(a => a.Brand)
                .WithMany()
                .HasForeignKey(a => a.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Model)
                .WithMany()
                .HasForeignKey(a => a.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.ContactName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.ContactPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.ContactEmail)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(a => a.Seller)
                .WithMany()
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ReviewByAdmin)
               .WithMany()
               .HasForeignKey(a => a.ReviewByAdminId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<int>();

            // Soft-deleted ads are excluded everywhere by default; DeleteAdvertisementAsync
            // sets the flag instead of removing the row.
            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.HasIndex(a => a.Price);
            builder.HasIndex(a => a.Year);
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.BrandId);
            builder.HasIndex(a => a.ModelId);
            builder.HasIndex(a => a.CityId);
            builder.HasIndex(a => a.SellerId);
            builder.HasIndex(a => a.ReviewByAdminId);
        }
    }
}
