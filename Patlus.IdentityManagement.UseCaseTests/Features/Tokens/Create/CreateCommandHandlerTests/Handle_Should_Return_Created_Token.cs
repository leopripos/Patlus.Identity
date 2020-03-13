using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Services;
using Patlus.IdentityManagement.UseCaseTests.Features.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Create.CreateCommandHandlerTests
{
    [Trait("UT-Feature", "Tokens/Create")]
    [Trait("UT-Class", "Tokens/Create/CreateCommandHandlerTests")]
    public sealed class Handle_Should_Return_Created_Token : IDisposable
    {
        private readonly Mock<ILogger<CreateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Return_Created_Token()
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

        [Theory(DisplayName = nameof(Handle_Should_Return_Created_Token))]
        [ClassData(typeof(TestData))]
        public async void Theory(Token expectedResult, CreateCommand command)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Identities).Returns(IdentitiesFaker.CreateIdentities().Values.AsQueryable());
            _mockTimeService.SetupGet(e => e.Now).Returns(expectedResult.CreatedTime);
            _mockPasswordService.Setup(e => e.ValidatePasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((p1, p2) => p1 == p2);
            _mockTokenService.Setup(e => e.Create(It.IsAny<Guid>(), It.IsAny<List<Claim>>())).Returns(expectedResult);

            var handler = new CreateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockPasswordService.Object,
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
                    It.Is<CreatedNotification>(e => (
                       e.Entity == actualResult
                       && e.Time == actualResult.CreatedTime
                   )),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        class TestData : TheoryData<Token, CreateCommand>
        {
            public TestData()
            {
                Add(
                    new Token() { 
                        Scheme = "Bearer",
                        Access = "jwtAccessTokenDummy",
                        Refresh = "jwtRefreshTokenTokenDummy",
                        CreatedTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1))
                    },
                    new CreateCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "sysadmin0",
                        Password = "sysadminpassword0",
                        RequestorId = null
                    }
                );

                Add(
                    new Token()
                    {
                        Scheme = "Bearer",
                        Access = "jwtAccessTokenDummy2",
                        Refresh = "jwtRefreshTokenTokenDumm2",
                        CreatedTime = new DateTimeOffset(2019, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1))
                    },
                    new CreateCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "sysadmin1",
                        Password = "sysadminpassword1",
                        RequestorId = null
                    }
                );
            }
        }
    }
}
