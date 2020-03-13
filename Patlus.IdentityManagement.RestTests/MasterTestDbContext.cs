using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Persistence.Configurations;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.RestTests
{
    public class MasterTestDbContext : DbContext, IMasterDbContext
    {
        protected readonly ITimeService TimeService;
        protected readonly IPasswordService PasswordService;

        public MasterTestDbContext(DbContextOptions<MasterTestDbContext> options, ITimeService timeService, IPasswordService passwordService)
            : base(options)
        {
            TimeService = timeService;
            PasswordService = passwordService;
        }

        public IQueryable<Pool> Pools
        {
            get { return Set<Pool>(); }
        }

        public IQueryable<Identity> Identities
        {
            get { return Set<Identity>(); }
        }

        public IQueryable<HostedAccount> HostedAccounts
        {
            get { return Set<HostedAccount>(); }
        }

        void IMasterDbContext.Add<TEntity>(TEntity entity)
        {
            Set<TEntity>().Add(entity);
        }

        void IMasterDbContext.Update<TEntity>(TEntity entity)
        {
            Set<TEntity>().Update(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PoolConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            modelBuilder.ApplyConfiguration(new HostedAccountConfiguration());

            var identityId = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var createdTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1));
            var lastUpdatedTime = new DateTimeOffset(2018, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1));

            modelBuilder.Entity<Pool>().HasData(new Pool[] {
                new Pool() {
                    Id = poolId,
                    Active = true,
                    Name = "Administrator Pool",
                    Description = "Default identity pool for system administrator.",
                    CreatorId = identityId,
                    CreatedTime = createdTime,
                    LastModifiedTime = lastUpdatedTime,
                }
            });

            modelBuilder.Entity<Identity>().HasData(new Identity[] {
                new Identity() {
                    Id = identityId,
                    AuthKey = Guid.NewGuid(),
                    PoolId = poolId,
                    Name = "Root Administrator",
                    Active = true,
                    CreatorId = identityId,
                    CreatedTime = createdTime,
                    LastModifiedTime = lastUpdatedTime,
                }
            });

            modelBuilder.Entity<HostedAccount>().HasData(new HostedAccount[] {
                new HostedAccount(){
                    Id = identityId,
                    Name = "root",
                    Password = PasswordService.GeneratePasswordHash("root"),
                    CreatorId = identityId,
                    CreatedTime = createdTime,
                    LastModifiedTime = lastUpdatedTime,
                }
            });
        }
    }
}
