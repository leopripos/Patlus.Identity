using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetOne;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetOne.GetOneQueryHandlerTests
{
    [Trait("UT-Feature", "Pools/GetOne")]
    [Trait("UT-Class", "Pools/GetOne/GetOneQueryHandlerTests")]
    public sealed class Handle_Should_Return_Requested_Pools : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Handle_Should_Return_Requested_Pools()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Return_Requested_Pools))]
        [ClassData(typeof(TestData))]
        public async void Handle_Should_Return_Total_Requested_Identities(Pool expectedResult, GetOneQuery query)
        {
            // Arrange
            var dataSource = PoolsFaker.CreatePools();
            _mockMasterDbContext.Setup(e => e.Pools).Returns(dataSource);

            var handler = new GetOneQueryHandler(_mockMasterDbContext.Object);

            // Act
            var actualResult = await handler.Handle(query, default);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult, opt => opt.IgnoringCyclicReferences());
        }

        class TestData : TheoryData<Pool?, GetOneQuery>
        {
            public TestData()
            {
                var dataSource = PoolsFaker.CreatePools();
                var requestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7");
                Expression<Func<Pool, bool>> condition;

                condition = e => e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49");
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );


                condition = e => e.Active == true;
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name == "System Administrator";
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name.Contains("Admin");
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name.Contains("e");
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name.Length == "System Administrator".Length;
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Archived == true;
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );


                condition = e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120));
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );


                condition = e => (
                    e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 0, 0, 0, 0, TimeSpan.Zero)
                    && e.CreatedTime <= new DateTimeOffset(2019, 10, 12, 0, 0, 0, 0, TimeSpan.Zero)
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => (
                    e.CreatedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120))
                    && e.CreatedTime <= new DateTimeOffset(2019, 10, 12, 2, 0, 0, 0, TimeSpan.FromMinutes(120))
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120));
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => (
                    e.LastModifiedTime >= new DateTimeOffset(2019, 10, 9, 2, 0, 0, 0, TimeSpan.FromMinutes(120))
                    && e.LastModifiedTime <= new DateTimeOffset(2019, 10, 12, 2, 0, 0, 0, TimeSpan.FromMinutes(120))
                );
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.CreatorId == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605");
                Add(
                    dataSource.Where(condition).FirstOrDefault(),
                    new GetOneQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );
            }
        }
    }
}
