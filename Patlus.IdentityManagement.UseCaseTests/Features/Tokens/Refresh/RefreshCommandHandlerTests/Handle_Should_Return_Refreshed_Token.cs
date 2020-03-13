using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Security;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using Patlus.IdentityManagement.UseCase.Services;
using Patlus.IdentityManagement.UseCaseTests.Features.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Refresh.RefreshCommandHandlerTests
{
    [Trait("UT-Feature", "Tokens/Refresh")]
    [Trait("UT-Class", "Tokens/Refresh/RefreshCommandHandlerTests")]
    public sealed class Handle_Should_Return_Refreshed_Token : IDisposable
    {
        private readonly Mock<ILogger<RefreshCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Return_Refreshed_Token()
        {
            _mockLogger = new Mock<ILogger<RefreshCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockTokenService = new Mock<ITokenService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockTokenService.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
        }

        [Theory(DisplayName = nameof(Handle_Should_Return_Refreshed_Token))]
        [ClassData(typeof(TestData))]
        public async void Theory(Token expectedResult, RefreshCommand command)
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.Pool,  new Guid("821e7913-876f-4377-a799-17fb8b5a0a49").ToString()),
                new Claim(SecurityClaimTypes.Subject, new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605").ToString())
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));

            _mockMasterDbContext.SetupGet(e => e.Identities).Returns(IdentitiesFaker.CreateIdentities().Values.AsQueryable());
            _mockTimeService.SetupGet(e => e.Now).Returns(expectedResult.CreatedTime);
            _mockTokenService.Setup(e => e.Create(It.IsAny<Guid>(), It.IsAny<List<Claim>>())).Returns(expectedResult);
            _mockTokenService.Setup(e => e.TryParseRefreshToken(It.IsAny<string>(), out principal)).Returns(true);
            _mockTokenService.Setup(e => e.ValidateRefreshToken(It.IsAny<Guid>(), It.IsAny<ClaimsPrincipal>())).Returns(true);

            var handler = new RefreshCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTokenService.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );
            // Act
            var actualResult = await handler.Handle(command, default);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.IgnoringCyclicReferences();

                return options;
            });
            ;
            _mockMediator.Verify(
                e => e.Publish(
                    It.Is<RefreshedNotification>(e => (
                       e.Entity == actualResult
                       && e.Time == actualResult.CreatedTime
                   )),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        class TestData : TheoryData<Token, RefreshCommand>
        {
            public TestData()
            {
                Add(
                    new Token()
                    {
                        Scheme = "Bearer",
                        Access = "jwtAccessTokenDummy",
                        Refresh = "jwtRefreshTokenTokenDummy",
                        CreatedTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1))
                    },
                    new RefreshCommand()
                    {
                        RefreshToken = "dummytoken",
                        RequestorId = null
                    }
                );
            }
        }
    }
}
