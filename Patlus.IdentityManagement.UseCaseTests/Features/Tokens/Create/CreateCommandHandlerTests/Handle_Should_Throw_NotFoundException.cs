using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Create.CreateCommandHandlerTests
{
    [Trait("UT-Feature", "Tokens/Create")]
    [Trait("UT-Class", "Tokens/Create/CreateCommandHandlerTests")]
    public sealed class Handle_Should_Throw_NotFoundException : IDisposable
    {
        private readonly Mock<ILogger<CreateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_NotFoundException()
        {
            _mockLogger = new Mock<ILogger<CreateCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockPasswordService.Reset();
            _mockTokenService.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_NotFoundException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedEntityName, object expectedEntityValue, CreateCommand command)
        {
            // Arrange
            var handler = new CreateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockPasswordService.Object,
                _mockTokenService.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<NotFoundException>().Where(e => (
                    e.EntityName == expectedEntityName
                    && e.EntityValue.ToString() == expectedEntityValue.ToString()
                )
            );
        }

        class TestData : TheoryData<string, object, CreateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                        Name = "sysadmin0",
                        Password = "****"
                    },
                    new CreateCommand()
                    {
                        PoolId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), // Invalid PoolId
                        Name = "sysadmin0",
                        Password = "sysadminpassword0",
                        RequestorId = null
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "invlidname",
                        Password = "****"
                    },
                    new CreateCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "invlidname", // Invalid Name
                        Password = "sysadminpassword0", // Invalid Name
                        RequestorId = null
                    }
                );

                Add(
                    nameof(Identity),
                    new
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "sysadmin0",
                        Password = "****"
                    },
                    new CreateCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "sysadmin0",
                        Password = "invalidpassword", // Invalid Password
                        RequestorId = null
                    }
                );
            }
        }
    }
}
