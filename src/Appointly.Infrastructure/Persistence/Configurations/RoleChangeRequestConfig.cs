using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointly.Infrastructure.Persistence.Configurations
{
    public class RoleChangeRequestConfig : IEntityTypeConfiguration<RoleChangeRequest>
    {
        public void Configure(EntityTypeBuilder<RoleChangeRequest> builder)
        {
            builder.ToTable("RoleChangeRequests");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.RequestedRole)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.RequestedAt)
                .IsRequired();

            builder.HasOne(r => r.User) 
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ReviewedByAdmin)
               .WithMany()
               .HasForeignKey(r => r.ReviewedByAdminId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
