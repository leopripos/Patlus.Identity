using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Create.CreateCommandHandlerTests
{
    [Trait("UT-Feature", "Pools/Create")]
    [Trait("UT-Class", "Pools/Create/CreateCommandHandlerTests")]
    public sealed class Handle_Should_Throw_NullArgumentException : IDisposable
    {
        private readonly Mock<ILogger<CreateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_NullArgumentException()
        {
            _mockLogger = new Mock<ILogger<CreateCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Throw_NullArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedParamName, CreateCommand command)
        {
            // Arrange
            var handler = new CreateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
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
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = null,
                        Description = "Development vendor description",
                        Active = true,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(CreateCommand.Description),
                    new CreateCommand()
                    {
                        Name = "Development Vendor",
                        Description = null,
                        Active = true,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(CreateCommand.Active),
                    new CreateCommand()
                    {
                        Name = "Development Vendor",
                        Description = "Development vendor description",
                        Active = null,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(CreateCommand.RequestorId),
                    new CreateCommand()
                    {
                        Name = "Development Vendor",
                        Description = "Development vendor description",
                        Active = true,
                        RequestorId = null,
                    }
                );
            }
        }
    }
}
