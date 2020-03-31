using AutoMapper;
using Moq;
using Patlus.Common.Presentation.Responses.Notifications;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers;
using Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Pools;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.IntegrationDispatcherTests.Dispatchers.Pools
{
    [Trait("UT-Class", "Dispatchers/Pools/UpdatedNotificationDispatcherTests")]
    public sealed class UpdatedNotificationDispatcherTests : IDisposable
    {
        private readonly Mock<IEventDispatcher> _mockEventDispatcher;
        private readonly IMapper _mapper;

        public UpdatedNotificationDispatcherTests()
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

            var notification = new UpdatedNotification(
                    new Pool() { Id = poolId },
                    new Dictionary<string, DeltaValue>() {
                        {
                            nameof(Pool.Name), 
                            new DeltaValue("Test", "Test Updated")
                        }
                    },
                    by,
                    time
                );

            var expectedDto = new UpdatedNotificationDto()
            {
                Id = poolId,
                Values = new Dictionary<string, DeltaValueDto>() {
                    {
                        nameof(Pool.Name),
                        new DeltaValueDto("Test", "Test Updated")
                    }
                },
                By = by,
                Time = time
            };

            var dispatcher = new UpdatedNotificationDispatcher(
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
                        It.Is<UpdatedNotificationDto>(e =>
                            e.Id == expectedDto.Id
                            && e.Values.Count == expectedDto.Values.Count
                            && e.Values[nameof(Pool.Name)].OldValue == expectedDto.Values[nameof(Pool.Name)].OldValue
                            && e.Values[nameof(Pool.Name)].NewValue == expectedDto.Values[nameof(Pool.Name)].NewValue
                            && e.By == expectedDto.By
                            && e.Time == expectedDto.Time
                        ),
                        It.Is<Guid>(e => e == notification.OrderingGroup),
                        It.Is<CancellationToken>(e => e == cancelartionToken)
                    ),
                Times.Once
            ); ;
        }
    }
}