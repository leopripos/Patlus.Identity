using Microsoft.EntityFrameworkCore;
using Patlus.Identity.UseCase.Services;
using System;
using System.Threading.Tasks;

namespace Patlus.Identity.Persistence.Services
{
    public class DbActivationService : IDbActivatorService
    {
        public Task<IDbInfo> Create(Guid id)
        {
            using var context = new ActivationDbContext(id);

            context.Database.EnsureCreated();

            var connection = context.Database.GetDbConnection();

            var dbInfo = new DbInfo()
            {
                ConnectionString = connection.ConnectionString,
            };

            return Task.FromResult(dbInfo as IDbInfo);
        }

        class DbInfo : IDbInfo
        {
            public string ConnectionString { get; set; }
        }

        class ActivationDbContext : DbContext
        {
            private readonly Guid poolId;

            public ActivationDbContext(Guid poolId)
            {
                this.poolId = poolId;
            }

            public string DatabaseServer
            {
                get
                {
                    return $"(localdb)\\MSSQLLocalDB";
                }
            }

            public string DatabaseName
            {
                get
                {
                    return $"Identity_{poolId}";
                }
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer($"Server={DatabaseServer};Database={DatabaseName};Trusted_Connection=True");
            }
        }
    }
}
