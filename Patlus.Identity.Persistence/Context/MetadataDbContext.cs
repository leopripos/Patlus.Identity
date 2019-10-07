using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using Patlus.Identity.Persistence.Configurations;
using Patlus.Identity.UseCase.Services;
using System.Linq;

namespace Patlus.Identity.Persistence.Contexts
{
    public class MetadataDbContext : DbContext, IMetadataDbContext
    {
        private readonly ITimeService timeService;

        public MetadataDbContext(DbContextOptions<MetadataDbContext> options, ITimeService timeService)
            : base(options)
        {
            this.timeService = timeService;
        }

        public IQueryable<Pool> Pools
        {
            get { return Set<Pool>(); }
        }

        void IMetadataDbContext.Add<TEntity>(TEntity entity)
        {
            Set<TEntity>().Add(entity);
        }

        void IMetadataDbContext.Update<TEntity>(TEntity entity)
        {
            Set<TEntity>().Update(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PoolConfiguration());
            modelBuilder.ApplyConfiguration(new PoolDatabaseConfiguration());

            var poolId = new System.Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var creatorId = new System.Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Identity_Administrator;Trusted_Connection=True";

            modelBuilder.Entity<Pool>().HasData(new Pool[] {
                new Pool() {
                    Id = poolId,
                    Active = true,
                    CreatorId = creatorId,
                    CreatedTime = timeService.Now,
                    LastModifiedTime = timeService.Now,
                }
            });

            modelBuilder.Entity<PoolDatabase>().HasData(new PoolDatabase[] {
                new PoolDatabase() {
                    Id = poolId,
                    ConnectionString = connectionString,
                    CreatorId = creatorId,
                    CreatedTime = timeService.Now,
                    LastModifiedTime = timeService.Now,
                }
            });
        }
    }
}
