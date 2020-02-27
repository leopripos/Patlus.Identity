using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Identities.Exist;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.Exist.ExistQueryHandlerTests
{
    [Trait("UT-Feature", "Identities/Exist")]
    [Trait("UT-Class", "Identities/Exist/ExistQueryHandlerTests")]
    public class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Handle_Should_Throw_NullArgumentException()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_NullArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, ExistQuery query)
        {
            // Arrange
            var handler = new ExistQueryHandler(_mockMasterDbContext.Object);

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, ExistQuery>
        {
            public TestData()
            {
                Add(
                    nameof(ExistQuery.Condition),
                    new ExistQuery()
                    {
                        Condition = null,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(ExistQuery.RequestorId),
                    new ExistQuery()
                    {
                        Condition = e => true,
                        RequestorId = null
                    }
                );
            }
        }
    }
}
