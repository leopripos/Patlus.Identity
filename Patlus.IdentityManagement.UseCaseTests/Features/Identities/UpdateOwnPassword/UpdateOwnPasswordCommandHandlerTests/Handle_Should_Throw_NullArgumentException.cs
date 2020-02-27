using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateOwnPassword.UpdateOwnPasswordCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/UpdateOwnPassword")]
    [Trait("UT-Class", "Identities/UpdateOwnPassword/UpdateOwnPasswordCommandHandlerTests")]
    public class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<ILogger<UpdateOwnPasswordCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_NullArgumentException()
        {
            _mockLogger = new Mock<ILogger<UpdateOwnPasswordCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
            _mockPasswordService.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_NullArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, UpdateOwnPasswordCommand command)
        {
            // Arrange
            var handler = new UpdateOwnPasswordCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<ArgumentException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, UpdateOwnPasswordCommand>
        {
            public TestData()
            {
                Add(
                    nameof(UpdateOwnPasswordCommand.RequestorId),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = "oldpassword",
                        NewPassword = "newpassword",
                        RequestorId = null,
                    }
                );
            }
        }
    }
}
