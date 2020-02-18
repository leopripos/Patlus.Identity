using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryHandlerTests
{
    public class Handle_Should_Return_Requested_Identities : IDisposable
    {
        private readonly IQueryable<Identity> _dataSource;

        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Return_Requested_Identities()
        {
            _dataSource = IdentitiesFaker.CreateIdentities().Values.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public async void Handle_Should_Return_Total_Requested_Identities(Identity expectedResult, GetOneBySecretQuery query)
        {
            // Arrange
            _mockMasterDbContext.Setup(e => e.Identities).Returns(_dataSource);
            _mockPasswordService.Setup(e => e.ValidatePasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns((string p1, string p2) => p1 == p2);

            var handler = new GetOneBySecretQueryHandler(
                _mockMasterDbContext.Object,
                _mockPasswordService.Object
            );

            // Act
            var actualResult = await handler.Handle(query, default);

            //Asert
            actualResult.Should().BeEquivalentTo(expectedResult, opt => opt.IgnoringCyclicReferences());
        }

        class TestData : TheoryData<Identity, GetOneBySecretQuery>
        {
            public TestData()
            {
                var dataSource = IdentitiesFaker.CreateIdentities().Values.AsQueryable();

                Expression<Func<Identity, bool>> condition;
                Guid poolId;
                string name;
                string password;

                poolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49");
                name = "sysadmin0";
                password = "sysadminpassword0";
                condition = e => (
                    e.PoolId == poolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false && e.HostedAccount.Name == name && e.HostedAccount.Password == password
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneBySecretQuery()
                    {
                        PoolId = poolId,
                        Name = name,
                        Password = password,
                    }
                );

                poolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49");
                name = "sysadmin1";
                password = "sysadminpassword1";
                condition = e => (
                    e.PoolId == poolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false && e.HostedAccount.Name == name && e.HostedAccount.Password == password
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneBySecretQuery()
                    {
                        PoolId = poolId,
                        Name = name,
                        Password = password,
                    }
                );

                poolId = new Guid("29899885-bbf1-430f-b9d6-613066b4021a");
                name = "employee0";
                password = "employeepassword0";
                condition = e => (
                    e.PoolId == poolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false && e.HostedAccount.Name == name && e.HostedAccount.Password == password
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneBySecretQuery()
                    {
                        PoolId = poolId,
                        Name = name,
                        Password = password,
                    }
                );
            }
        }
    }
}
