using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patlus.Identity.UseCase.Entities;

namespace Patlus.Identity.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
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
