using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetAll;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetAll.GetAllQueryHandlerTests
{
    public class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Handle_Should_Throw_NullArgumentException()
        {
            this._mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        public void Dispose()
        {
            this._mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, GetAllQuery query)
        {
            // Arrange
            var handler = new GetAllQueryHandler(this._mockMasterDbContext.Object);

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, GetAllQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetAllQuery.RequestorId),
                    new GetAllQuery()
                    {
                        RequestorId = null
                    }
                );
            }
        }
    }
}
