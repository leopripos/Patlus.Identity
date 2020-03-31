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
    public sealed class Handle_Should_Throw_ArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IIdentifierService> _mockIdentifierService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_ArgumentException()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockIdentifierService = new Mock<IIdentifierService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
            _mockIdentifierService.Reset();
            _mockMediator.Reset();
            _mockMediator.Reset();
            _mockPasswordService.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_ArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, UpdateOwnPasswordCommand command)
        {
            // Arrange
            var handler = new UpdateOwnPasswordCommandHandler(
                _mockMasterDbContext.Object,
                _mockIdentifierService.Object,
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
                    nameof(UpdateOwnPasswordCommand.OldPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = null,
                        NewPassword = "newpassword",
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(UpdateOwnPasswordCommand.OldPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = string.Empty,
                        NewPassword = "newpassword",
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(UpdateOwnPasswordCommand.NewPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = "oldpassword",
                        NewPassword = null,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(UpdateOwnPasswordCommand.NewPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = "oldpassword",
                        NewPassword = string.Empty,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );
            }
        }
    }
}
