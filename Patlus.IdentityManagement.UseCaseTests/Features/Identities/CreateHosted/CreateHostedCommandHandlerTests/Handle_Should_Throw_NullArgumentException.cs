using FluentAssertions;
using MediatR;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/CreateHosted")]
    [Trait("UT-Class", "Identities/CreateHosted/CreateHostedCommandHandlerTests")]
    public sealed class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IIdentifierService> _mockIdentifierService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_NullArgumentException()
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
            _mockTimeService.Reset();
            _mockMediator.Reset();
            _mockPasswordService.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, CreateHostedCommand command)
        {
            // Arrange
            var handler = new CreateHostedCommandHandler(
                _mockMasterDbContext.Object,
                _mockIdentifierService.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == expectedParamName);
        }

        class TestData : TheoryData<string, CreateHostedCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateHostedCommand.PoolId),
                    new CreateHostedCommand()
                    {
                        PoolId = null,
                        Name = "New User Name",
                        AccountName = "newuser",
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.Name),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = null,
                        AccountName = "newuser",
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountName),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New User Name",
                        AccountName = null,
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountPassword),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New User Name",
                        AccountName = "newuser",
                        AccountPassword = null,
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.Active),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New User Name",
                        AccountName = "newuser",
                        AccountPassword = "newpassword2",
                        Active = null,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.RequestorId),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New User Name",
                        AccountName = "newuser",
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = null,
                    }
                );
            }
        }
    }
}
