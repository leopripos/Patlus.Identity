using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryHandlerTests
{
    public class Handle_Should_Throw_NotFoundException : IDisposable
    {
        private readonly IQueryable<Pool> _poolDataSource;
        private readonly IQueryable<Identity> _identityDataSource;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_NotFoundException()
        {
            var pools = new List<Pool>
            {
                new Pool() {
                    Id = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                    Active = true,
                    Archived = false,
                },
                new Pool() {
                    Id = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2"),
                    Active = false,
                    Archived = false,
                },
                new Pool() {
                    Id = new Guid("e599ecfa-0c7f-402f-a1ae-f3c936d0824b"),
                    Active = true,
                    Archived = true,
                }
            };

            var identities = new List<Identity>()
            {
                new Identity()
                {
                    Id =  Guid.NewGuid(),
                    Active = true,
                    Archived = false,
                    PoolId =  pools[0].Id,
                    Pool = pools[0],
                    HostedAccount = new HostedAccount() {
                        Id = Guid.NewGuid(),
                        Name = "sysadmin0",
                        Password = "sysadmin0pass",
                    }
                },
                new Identity()
                {
                    Id =  Guid.NewGuid(),
                    Active = false,
                    Archived = false,
                    PoolId =  pools[0].Id,
                    Pool = pools[0],
                    HostedAccount = new HostedAccount() {
                        Id = Guid.NewGuid(),
                        Name = "sysadmin1",
                        Password = "sysadmin1pass",
                    }
                },
                new Identity()
                {
                    Id =  Guid.NewGuid(),
                    Active = true,
                    Archived = true,
                    PoolId =  pools[0].Id,
                    Pool = pools[0],
                    HostedAccount = new HostedAccount() {
                        Id = Guid.NewGuid(),
                        Name = "sysadmin2",
                        Password = "sysadmin2pass",
                    }
                },
                new Identity()
                {
                    Id =  Guid.NewGuid(),
                    Active = true,
                    Archived = false,
                    PoolId =  pools[1].Id,
                    Pool = pools[1],
                    HostedAccount = new HostedAccount() {
                        Id = Guid.NewGuid(),
                        Name = "sysadmin3",
                        Password = "sysadmin3pass",
                    }
                },
                new Identity()
                {
                    Id =  Guid.NewGuid(),
                    Active = true,
                    Archived = false,
                    PoolId =  pools[2].Id,
                    Pool = pools[2],
                    HostedAccount = new HostedAccount() {
                        Id = Guid.NewGuid(),
                        Name = "sysadmin4",
                        Password = "sysadmin4pass",
                    }
                },
            };

            _poolDataSource = pools.AsQueryable();
            _identityDataSource = identities.AsQueryable();

            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedEntityName, object expectedEntityValue, GetOneBySecretQuery query)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_poolDataSource);
            _mockMasterDbContext.SetupGet(e => e.Identities).Returns(_identityDataSource);
            _mockPasswordService.Setup(e => e.ValidatePasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns((string p1, string p2) => p1 == p2);

            var handler = new GetOneBySecretQueryHandler(
                _mockMasterDbContext.Object,
                _mockPasswordService.Object
            );

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<NotFoundException>().Where(e => (
                    e.EntityName == expectedEntityName
                    && e.EntityValue.ToString() == expectedEntityValue.ToString()
                )
            );
        }

        class TestData : TheoryData<string, object, GetOneBySecretQuery>
        {
            public TestData()
            {
                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin0",
                        Password = "sysadmin1pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin0",
                        Password = "sysadmin1pass"
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin1",
                        Password = "sysadmin1pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin1",
                        Password = "sysadmin1pass"
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin0",
                        Password = "sysadmin1pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin0",
                        Password = "sysadmin1pass"
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin2",
                        Password = "sysadmin2pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Name = "sysadmin2",
                        Password = "sysadmin2pass"
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2"),
                        Name = "sysadmin3",
                        Password = "sysadmin3pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2"),
                        Name = "sysadmin3",
                        Password = "sysadmin3pass"
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("e599ecfa-0c7f-402f-a1ae-f3c936d0824b"),
                        Name = "sysadmin4",
                        Password = "sysadmin4pass"
                    },
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("e599ecfa-0c7f-402f-a1ae-f3c936d0824b"),
                        Name = "sysadmin4",
                        Password = "sysadmin4pass"
                    }
                );
            }
        }
    }
}
