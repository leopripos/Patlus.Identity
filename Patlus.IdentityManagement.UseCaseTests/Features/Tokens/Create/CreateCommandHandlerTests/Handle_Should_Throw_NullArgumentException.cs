using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Create.CreateCommandHandlerTests
{
    [Trait("UT-Feature", "Tokens/Create")]
    [Trait("UT-Class", "Tokens/Create/CreateCommandHandlerTests")]
    public sealed class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_NullArgumentException()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
            _mockPasswordService.Reset();
            _mockTokenService.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_NullArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, CreateCommand command)
        {
            // Arrange
            var handler = new CreateCommandHandler(
                _mockMasterDbContext.Object,
                _mockPasswordService.Object,
                _mockTokenService.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, CreateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateCommand.PoolId),
                    new CreateCommand()
                    {
                        PoolId = null,
                        Name = "root",
                        Password = "password",
                        RequestorId = null
                    }
                );

                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        PoolId = Guid.NewGuid(),
                        Name = null,
                        Password = "password",
                        RequestorId = null
                    }
                );

                Add(
                    nameof(CreateCommand.Password),
                    new CreateCommand()
                    {
                        PoolId = Guid.NewGuid(),
                        Name = "root",
                        Password = null,
                        RequestorId = null
                    }
                );
            }
        }
    }
}
