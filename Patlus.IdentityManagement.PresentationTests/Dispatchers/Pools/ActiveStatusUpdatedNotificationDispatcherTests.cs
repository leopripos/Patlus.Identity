using AutoMapper;
using Moq;
using Patlus.Common.Presentation.Services;
using Patlus.IdentityManagement.Presentation.Dispatchers;
using Patlus.IdentityManagement.Presentation.Dispatchers.Pools;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.PresentationTests.Dispatchers.Pools
{
    [Trait("UT-Class", "Dispatchers/Pools/ActiveStatusUpdatedNotificationDispatcherTests")]
    public sealed class ActiveStatusUpdatedNotificationDispatcherTests : IDisposable
    {
        private readonly Mock<IEventDispatcher> _mockEventDispatcher;
        private readonly IMapper _mapper;

        public ActiveStatusUpdatedNotificationDispatcherTests()
        {
            _mockEventDispatcher = new Mock<IEventDispatcher>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper(); ;
        }

        public void Dispose()
        {
            _mockEventDispatcher.Reset();
        }

        [Fact()]
        public async Task Handle_Should_Dispatch_Right_Data()
        {
            // Arrange
            var poolId = new Guid("ce818a95-a5bc-4901-ad51-00b964efc7c0");
            var by = new Guid("2442cb04-5552-487c-ad31-ebc4ad990e84");
            var time = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);

            var notification = new ActiveStatusUpdatedNotification(
                    new Pool() { Id = poolId },
                    true,
                    false,
                    by,
                    time
                );

            var expectedDto = new ActiveStatusUpdatedNotificationDto()
            {
                Id = poolId,
                OldVaiue = true,
                NewValue = false,
                By = by,
                Time = time
            };

            var dispatcher = new ActiveStatusUpdatedNotificationDispatcher(
                    _mockEventDispatcher.Object,
                    _mapper
                );

            var cancelartionToken = new CancellationToken();

            // Act
            await dispatcher.Handle(notification, cancelartionToken);

            // Assert
            _mockEventDispatcher.Verify(
                d => d.DispatchAsync(
                        It.Is<string>(e => e == Topics.Pools),
                        It.Is<ActiveStatusUpdatedNotificationDto>(e =>
                            e.Id == expectedDto.Id
                            && e.OldVaiue == expectedDto.OldVaiue
                            && e.NewValue == expectedDto.NewValue
                            && e.By == expectedDto.By
                            && e.Time == expectedDto.Time
                        ),
                        It.Is<Guid>(e => e == notification.OrderingGroup),
                        It.Is<CancellationToken>(e => e == cancelartionToken)
                    ),
                Times.Once
            );
        }
    }
}