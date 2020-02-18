using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Upda.UpdateCommandHandlerTests
{
    public class Handle_Should_Throw_NotFoundException : IDisposable
    {
        private readonly Mock<ILogger<UpdateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Throw_NotFoundException()
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

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedEntityName, object expectedEntityValue, UpdateCommand command)
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
            action.Should().Throw<NotFoundException>().Where(e => (
                    e.EntityName == expectedEntityName
                    && e.EntityValue.ToString() == expectedEntityValue.ToString()
                )
            );
        }

        class TestData : TheoryData<string, object, UpdateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(Pool),
                    new { 
                        Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")
                    },
                    new UpdateCommand()
                    {
                        Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                        Name = "Test Name",
                        Description = "Test Description"
                    }
                );
            }
        }
    }
}
