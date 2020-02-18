using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetAll;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetAll.GetAllQueryHandlerTests
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
        public void Theory(string expectedParamName, GetAllQuery query)
        {
            // Arrange
            var handler = new GetAllQueryHandler(this.mockMasterDbContext.Object);

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
