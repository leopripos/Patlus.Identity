using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryHandlerTests
{
    public class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_NullArgumentException()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, GetOneBySecretQuery query)
        {
            // Arrange
            var handler = new GetOneBySecretQueryHandler(
                _mockMasterDbContext.Object,
                _mockPasswordService.Object
            );

            // Act
            Action action = () => handler.Handle(query, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, GetOneBySecretQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetOneBySecretQuery.PoolId),
                    new GetOneBySecretQuery()
                    {
                        PoolId = null,
                        Name = "leopripos",
                        Password = "leopripos"
                    }
                );

                Add(
                    nameof(GetOneBySecretQuery.Name),
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = null,
                        Password = "leopripos"
                    }
                );

                Add(
                    nameof(GetOneBySecretQuery.Password),
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "leopripos",
                        Password = null
                    }
                );
            }
        }
    }
}
