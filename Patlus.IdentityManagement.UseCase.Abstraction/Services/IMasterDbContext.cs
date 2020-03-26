using Patlus.IdentityManagement.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Services
{
    public interface IMasterDbContext
    {
        IQueryable<Pool> Pools { get; }
        IQueryable<Identity> Identities { get; }
        IQueryable<HostedAccount> HostedAccounts { get; }

        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
