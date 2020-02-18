using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Count;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Count.CountQueryHandlerTests
{
    public class Handle_Should_Return_Total_Requested_Pools : IDisposable
    {
        private readonly IQueryable<Pool> _dataSource;

        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Handle_Should_Return_Total_Requested_Pools()
        {
            _dataSource = PoolsFaker.CreatePools().Values.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public async void Theory(int expectedResult, CountQuery query)
        {
            // Arrange
            _mockMasterDbContext.Setup(e => e.Pools).Returns(_dataSource);

            var handler = new CountQueryHandler(_mockMasterDbContext.Object);

            // Act
            var actualResult = await handler.Handle(query, default);

            //Asert
            actualResult.Should().Be(expectedResult);
        }

        class TestData : TheoryData<int, CountQuery>
        {
            public TestData()
            {
                var dataSource = PoolsFaker.CreatePools().Values.AsQueryable();
                var requestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7");
                Expression<Func<Pool, bool>> condition = null;

                condition = null;
                Add(
                    dataSource.Count(),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49");
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Active == true;
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name == "System Admin 0";
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name.Contains("System Admin");
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Name.Length == 14;
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.Archived == true;
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );


                condition = e => e.CreatedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120));
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
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
                    dataSource.Count(condition),
                    new CountQuery()
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
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 0, 0, 0, 0, TimeSpan.Zero);
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.LastModifiedTime == new DateTimeOffset(2019, 10, 10, 2, 0, 0, 0, TimeSpan.FromMinutes(120));
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
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
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );

                condition = e => e.CreatorId == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605");
                Add(
                    dataSource.Count(condition),
                    new CountQuery()
                    {
                        Condition = condition,
                        RequestorId = requestorId
                    }
                );
            }
        }
    }
}
