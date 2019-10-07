using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patlus.Identity.UseCase.Entities;

namespace Patlus.Identity.Persistence.Configurations
{
    public class PoolConfiguration : IEntityTypeConfiguration<Pool>
    {
        public void Configure(EntityTypeBuilder<Pool> builder)
        {
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(e => e.Active)
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
        }
    }
}
