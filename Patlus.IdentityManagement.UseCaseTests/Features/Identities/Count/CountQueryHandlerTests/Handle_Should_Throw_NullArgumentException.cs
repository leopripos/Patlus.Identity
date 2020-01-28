using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Identities.Count;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.Count.CountQueryHandlerTests
{
    public class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private Mock<IMasterDbContext> mockMasterDbContext;

        public Handle_Should_Throw_NullArgumentException()
        {
            this.mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        public void Dispose()
        {
            this.mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, CountQuery query)
        {
            // Arrange
            var handler = new CountQueryHandler(this.mockMasterDbContext.Object);

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, CountQuery>
        {
            public TestData()
            {
                Add(
                    nameof(CountQuery.RequestorId),
                    new CountQuery()
                    {
                        Condition = e => true,
                        RequestorId = null
                    }
                );
            }
        }
    }
}
