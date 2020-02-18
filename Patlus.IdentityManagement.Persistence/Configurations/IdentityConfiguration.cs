using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Persistence.Configurations
{
    public class IdentityConfiguration : IEntityTypeConfiguration<Identity>
    {
        public void Configure(EntityTypeBuilder<Identity> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(e => e.AuthKey)
                .IsRequired();

            builder.Property(e => e.Active)
                .IsRequired();

            builder.Property(e => e.Name)
                .IsRequired();

            builder.Property(e => e.CreatorId)
                .IsRequired();

            builder.Property(e => e.CreatedTime)
                .IsRequired();

            builder.Property(e => e.LastModifiedTime)
                .IsRequired();

            builder.Property(e => e.Archived)
                .IsRequired();

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.AuthKey).IsUnique();

            builder.HasIndex(e => e.Name).IsUnique();

            builder.HasOne(e => e.Pool)
                .WithOne()
                .HasForeignKey<Identity>(e => e.PoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
