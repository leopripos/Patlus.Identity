using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOne.GetOneQueryHandlerTests
{
    [Trait("UT-Feature", "Identities/GetOne")]
    [Trait("UT-Class", "Identities/GetOne/GetOneQueryHandlerTests")]
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

        [Theory(DisplayName = nameof(Handle_Should_Throw_NullArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, GetOneQuery query)
        {
            // Arrange
            var handler = new GetOneQueryHandler(this.mockMasterDbContext.Object);

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, GetOneQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetOneQuery.Condition),
                    new GetOneQuery()
                    {
                        Condition = null,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(GetOneQuery.RequestorId),
                    new GetOneQuery()
                    {
                        Condition = e => true,
                        RequestorId = null
                    }
                );
            }
        }
    }
}
