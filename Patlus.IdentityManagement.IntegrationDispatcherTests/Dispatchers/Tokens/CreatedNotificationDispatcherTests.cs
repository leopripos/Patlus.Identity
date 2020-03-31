using AutoMapper;
using Moq;
using Patlus.Common.Presentation.Services;
using Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers;
using Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Tokens;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.IntegrationDispatcherTests.Dispatchers.Tokens
{
    [Trait("UT-Class", "Dispatchers/Tokens/CreatedNotificationDispatcherTests")]
    public sealed class CreatedNotificationDispatcherTests : IDisposable
    {
        private readonly Mock<IEventDispatcher> _mockEventDispatcher;
        private readonly IMapper _mapper;

        public CreatedNotificationDispatcherTests()
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
            var identityId = new Guid("ce818a95-a5bc-4901-ad51-00b964efc7c0");
            var tokenId = new Guid("ce818a95-a5bc-4901-ad51-00b964efc7c0");
            var by = new Guid("2442cb04-5552-487c-ad31-ebc4ad990e84");
            var time = new DateTimeOffset(2020, 1, 1, 1, 1, 1, TimeSpan.Zero);

            var notification = new CreatedNotification(
                    identityId,
                    new Token() { 
                        Id = tokenId,
                        Scheme = "Bearer",
                        Access = "accessToken",
                        Refresh = "refreshToken",
                        CreatedTime = time
                    },
                    by,
                    time
                );

            var expectedDto = new CreatedNotificationDto()
            {
                IdentityId = identityId,
                Id = tokenId,
                Scheme = "Bearer",
                Access = "accessToken",
                Refresh = "refreshToken",
                By = by,
                Time = time
            };

            var dispatcher = new CreatedNotificationDispatcher(
                    _mockEventDispatcher.Object,
                    _mapper
                );

            var cancelartionToken = new CancellationToken();

            // Act
            await dispatcher.Handle(notification, cancelartionToken);

            // Assert
            _mockEventDispatcher.Verify(
                d => d.DispatchAsync(
                        It.Is<string>(e => e == Topics.Tokens),
                        It.Is<CreatedNotificationDto>(e =>
                            e.IdentityId == expectedDto.IdentityId
                            && e.Id == expectedDto.Id
                            && e.Scheme == expectedDto.Scheme
                            && e.Access == expectedDto.Access
                            && e.Refresh == expectedDto.Refresh
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