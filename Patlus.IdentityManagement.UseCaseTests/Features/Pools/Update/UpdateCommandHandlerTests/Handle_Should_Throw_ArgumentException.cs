using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Update.UpdateCommandHandlerTests
{
    [Trait("UT-Feature", "Pools/Update")]
    [Trait("UT-Class", "Pools/Update/UpdateCommandHandlerTests")]
    public class Handle_Should_Throw_ArgumentException : IDisposable
    {
        private readonly Mock<ILogger<UpdateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_ArgumentException()
        {
            _mockLogger = new Mock<ILogger<UpdateCommandHandler>>();
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

        [Theory(DisplayName = nameof(Handle_Should_Throw_ArgumentException))]
        [ClassData(typeof(TestData))]
        public void Theory(UpdateCommand command)
        {
            // Arrange
            var handler = new UpdateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<ArgumentException>().Where(e => (
                   e.Message == $"One of {nameof(command.Name)} or {nameof(command.Description)} should be filled."
                )
            );
        }

        class TestData : TheoryData<UpdateCommand>
        {
            public TestData()
            {
                Add(
                    new UpdateCommand()
                    {
                        Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );
            }
        }
    }
}
