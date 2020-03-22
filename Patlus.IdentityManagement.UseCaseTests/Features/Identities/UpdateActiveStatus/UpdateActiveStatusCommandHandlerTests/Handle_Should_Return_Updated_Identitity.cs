using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateActiveStatus.UpdateActiveStatusCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/UpdateActiveStatus")]
    [Trait("UT-Class", "Identities/UpdateActiveStatus/UpdateActiveStatusCommandHandlerTests")]
    public sealed class Handle_Should_Return_Updated_Identitity : IDisposable
    {
        private readonly Mock<ILogger<UpdateActiveStatusCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Return_Updated_Identitity()
        {
            _mockLogger = new Mock<ILogger<UpdateActiveStatusCommandHandler>>();
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
        public async void Theory(Identity previousValue, UpdateActiveStatusCommand command)
        {
            // Arrange
            var currentTime = DateTimeOffset.Now;
            _mockMasterDbContext.Setup(e => e.Identities).Returns(IdentitiesFaker.CreateIdentities().Values.AsQueryable());
            _mockTimeService.Setup(e => e.Now).Returns(currentTime);

            var handler = new UpdateActiveStatusCommandHandler(
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
                opt = opt.Excluding(e => e.Active);
                opt = opt.Excluding(e => e.LastModifiedTime);

                return opt;
            });

            actualResult.Active.Should().Be(command.Active!.Value);
            actualResult.LastModifiedTime.Should().Be(currentTime);

            _mockMediator.Verify(
                e => e.Publish(
                    It.Is<ActiveStatusUpdatedNotification>(notif => (
                        notif.Entity == actualResult
                        && notif.OldVaiue == previousValue.Active
                        && notif.NewValue == command.Active
                    )),
                    It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        class TestData : TheoryData<Identity?, UpdateActiveStatusCommand>
        {
            public TestData()
            {
                var dataSource = IdentitiesFaker.CreateIdentities().Values.AsQueryable();
                Add(
                    dataSource.Where(e => (
                        e.Id == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateActiveStatusCommand()
                    {
                        PoolId = null!,
                        Id = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Active = false,
                        RequestorId = Guid.NewGuid()
                    }
                );

                Add(
                    dataSource.Where(e => (
                        e.PoolId == new Guid("821e7913-876f-4377-a799-17fb8b5a0a49")
                        && e.Id == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateActiveStatusCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Id = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        Active = true,
                        RequestorId = Guid.NewGuid()
                    }
                );
            }
        }
    }
}
