using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patlus.Identity.UseCase.Entities;

namespace Patlus.Identity.Persistence.Configurations
{
    public class PoolDatabaseConfiguration : IEntityTypeConfiguration<PoolDatabase>
    {
        public void Configure(EntityTypeBuilder<PoolDatabase> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(e => e.ConnectionString)
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

            builder.HasOne(e => e.Pool)
                .WithOne(account => account.Database)
                .HasPrincipalKey<PoolDatabase>(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
