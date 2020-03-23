using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Refresh.RefreshCommandHandlerTests
{
    [Trait("UT-Feature", "Tokens/Refresh")]
    [Trait("UT-Class", "Tokens/Refresh/RefreshCommandHandlerTests")]
    public sealed class Handle_Should_Throw_ArgumentException : IDisposable
    {
        private readonly Mock<ILogger<RefreshCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_ArgumentException()
        {
            _mockLogger = new Mock<ILogger<RefreshCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockTokenService = new Mock<ITokenService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockTokenService.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_ArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, RefreshCommand command)
        {
            // Arrange
            ClaimsPrincipal? principal = null;
            _mockTokenService.Setup(e => e.TryParseRefreshToken(It.IsAny<string>(), out principal)).Returns(false);

            var handler = new RefreshCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTokenService.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, RefreshCommand>
        {
            public TestData()
            {
                Add(
                    nameof(RefreshCommand.RefreshToken),
                    new RefreshCommand()
                    {
                        RefreshToken = "dummytoken",
                        RequestorId = null
                    }
                );
            }
        }
    }
}
