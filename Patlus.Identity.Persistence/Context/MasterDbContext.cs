using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using Patlus.Identity.Persistence.Configurations;
using Patlus.Identity.UseCase.Services;
using System.Linq;

namespace Patlus.Identity.Persistence.Contexts
{
    public class MasterDbContext : DbContext, IMasterDbContext
    {
        private readonly ITimeService timeService;
        private readonly IPasswordService passwordService;

        public MasterDbContext(DbContextOptions<MasterDbContext> options, ITimeService timeService, IPasswordService passwordService)
            : base(options)
        {
            this.timeService = timeService;
            this.passwordService = passwordService;
        }

        public IQueryable<Account> Accounts
        {
            get { return Set<Account>(); }
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
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new HostedAccountConfiguration());

            var accountId = new System.Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");
            var accountName = "root";
            var accountPassword = "root";

            modelBuilder.Entity<Account>().HasData(new Account[] {
                new Account() {
                    Id = accountId,
                    Active = true,
                    CreatorId = accountId,
                    CreatedTime = timeService.Now,
                    LastModifiedTime = timeService.Now,
                }
            });

            modelBuilder.Entity<HostedAccount>().HasData(new HostedAccount [] {
                new HostedAccount(){
                    Id = accountId,
                    Name = accountName,
                    Password = passwordService.GeneratePasswordHash(accountPassword),
                    CreatorId = accountId,
                    CreatedTime = timeService.Now,
                    LastModifiedTime = timeService.Now,
                }
            });
        }
    }
}
