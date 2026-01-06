using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointly.Infrastructure.Persistence.Configurations
{
    internal class AdvertismentConfiguration : IEntityTypeConfiguration<Advertisment>
    {
        public void Configure(EntityTypeBuilder<Advertisment> builder)
        {
            builder.ToTable("Advertisments");

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

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasIndex(a => a.Price);
            builder.HasIndex(a => a.Year);
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.BrandId);
            builder.HasIndex(a => a.ModelId);
            builder.HasIndex(a => a.CityId);
        }
    }
}
