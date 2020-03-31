using MockQueryable.Moq;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools
{
    public static class PoolsFaker
    {
        public static IQueryable<Pool> CreatePools()
        {
            var admin0Id = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605");

            var createdDate1 = new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
            var createdDate2 = new DateTimeOffset(2019, 10, 12, 0, 0, 0, 0, TimeSpan.Zero);

            var sysAdminPool = new Pool()
            {
                Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                Active = true,
                Name = $"System Administrator",
                CreatorId = admin0Id,
                CreatedTime = createdDate1,
                LastModifiedTime = createdDate1,
                Archived = false,
            };

            var employeePool = new Pool()
            {
                Id = new Guid("29899885-bbf1-430f-b9d6-613066b4021a"),
                Active = true,
                Name = $"Employee",
                CreatorId = admin0Id,
                CreatedTime = createdDate2,
                LastModifiedTime = createdDate2,
                Archived = false,
            };

            var pools = new List<Pool>
            {
                sysAdminPool,
                employeePool
            };

            return pools.AsQueryable().BuildMock().Object;
        }
    }
}
