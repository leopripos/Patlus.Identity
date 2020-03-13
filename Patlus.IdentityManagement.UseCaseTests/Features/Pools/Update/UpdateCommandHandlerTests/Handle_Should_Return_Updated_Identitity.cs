using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Update.UpdateCommandHandlerTests
{
    [Trait("UT-Feature", "Pools/Update")]
    [Trait("UT-Class", "Pools/Update/UpdateCommandHandlerTests")]
    public sealed class Handle_Should_Return_Updated_Identitity : IDisposable
    {
        private readonly Mock<ILogger<UpdateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Return_Updated_Identitity()
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

        [Theory(DisplayName = nameof(Handle_Should_Return_Updated_Identitity))]
        [ClassData(typeof(TestData))]
        public async void Theory(Pool previousValue, UpdateCommand command)
        {
            // Arrange
            var currentTime = DateTimeOffset.Now;
            _mockMasterDbContext.Setup(e => e.Pools).Returns(PoolsFaker.CreatePools().Values.AsQueryable());
            _mockTimeService.Setup(e => e.Now).Returns(currentTime);

            var handler = new UpdateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            var actualResult = await handler.Handle(command, default);

            // Assert
            actualResult.Should().BeEquivalentTo(previousValue, opt =>
            {
                opt = opt.IgnoringCyclicReferences();
                if (command.HasName)
                {
                    opt = opt.Excluding(e => e.Name);
                }
                if (command.HasDescription)
                {
                    opt = opt.Excluding(e => e.Description);
                }
                opt = opt.Excluding(e => e.LastModifiedTime);

                return opt;
            });

            if (command.HasName)
            {
                actualResult.Name.Should().Be(command.Name);
            }

            if (command.HasDescription)
            {
                actualResult.Description.Should().Be(command.Description);
            }

            actualResult.LastModifiedTime.Should().Be(currentTime);

            _mockMediator.Verify(
                e => e.Publish(
                    It.Is<UpdatedNotification>(notif => (
                        notif.Entity == actualResult
                        && (
                            command.HasName == false
                            || (
                                ((string)notif.Values[nameof(actualResult.Name)].OldValue) == previousValue.Name
                                && ((string)notif.Values[nameof(actualResult.Name)].NewValue) == command.Name
                            )
                        )
                        && (
                            command.HasDescription == false
                            || (
                                ((string)notif.Values[nameof(actualResult.Description)].OldValue) == previousValue.Description
                                && ((string)notif.Values[nameof(actualResult.Description)].NewValue) == command.Description
                            )
                        )
                    )),
                    It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        class TestData : TheoryData<Pool, UpdateCommand>
        {
            public TestData()
            {
                var dataSource = PoolsFaker.CreatePools().Values.AsQueryable();
                Add(
                    dataSource.Where(e => (
                        e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateCommand()
                    {
                        Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New Name 1",
                        Description = "New Description 1",
                        RequestorId = Guid.NewGuid()
                    }
                );

                Add(
                    dataSource.Where(e => (
                        e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateCommand()
                    {
                        Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "New Name 2",
                        RequestorId = Guid.NewGuid()
                    }
                );

                Add(
                    dataSource.Where(e => (
                        e.Id == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateCommand()
                    {
                        Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Description = "New Description 2",
                        RequestorId = Guid.NewGuid()
                    }
                );
            }
        }
    }
}
