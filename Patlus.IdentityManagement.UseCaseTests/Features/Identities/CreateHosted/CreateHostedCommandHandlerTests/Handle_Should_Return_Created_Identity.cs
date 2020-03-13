using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/CreateHosted")]
    [Trait("UT-Class", "Identities/CreateHosted/CreateHostedCommandHandlerTests")]
    public sealed class Handle_Should_Return_Created_Identity : IDisposable
    {
        private readonly IQueryable<Identity> _identitiesDataSource;
        private readonly IQueryable<Pool> _poolsDataSource;

        private readonly Mock<ILogger<CreateHostedCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Return_Created_Identity()
        {
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Archived = false,
                },
            };

            var identities = new List<Identity>();

            _poolsDataSource = pools.AsQueryable();
            _identitiesDataSource = identities.AsQueryable();

            _mockLogger = new Mock<ILogger<CreateHostedCommandHandler>>();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockLogger.Reset();
            _mockMasterDbContext.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
            _mockPasswordService.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public async void Theory(Identity expectedResult, CreateHostedCommand query)
        {
            // Arrange
            _mockTimeService.SetupGet(e => e.Now).Returns(new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1)));
            _mockPasswordService.Setup(e => e.GeneratePasswordHash("newpassword1")).Returns("newpasswordhash1");
            _mockPasswordService.Setup(e => e.GeneratePasswordHash("newpassword2")).Returns("newpasswordhash2");
            _mockMasterDbContext.SetupGet(e => e.Identities).Returns(_identitiesDataSource);
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_poolsDataSource);

            var handler = new CreateHostedCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
            );

            // Act
            var actualResult = await handler.Handle(query, default);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.IgnoringCyclicReferences();
                options.Excluding(e => e.Id);
                options.Excluding(e => e.AuthKey);
                options.Excluding(e => e.HostedAccount.Id);

                return options;
            });

            _mockPasswordService.Verify(e => e.GeneratePasswordHash(It.IsAny<string>()), Times.Once);
            _mockMasterDbContext.Verify(e => e.Add(It.Is<Identity>(e => e == actualResult)), Times.Once);
            _mockMasterDbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(
                e => e.Publish(
                    It.Is<CreatedNotification>(e => (
                       e.Entity == actualResult
                       && e.By == actualResult.CreatorId
                       && e.Time == actualResult.CreatedTime
                   )),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        class TestData : TheoryData<Identity, CreateHostedCommand>
        {
            public TestData()
            {
                var currentTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1));
                Identity expectedIdentity;

                expectedIdentity = new Identity()
                {
                    PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Active = true,
                    Name = "New User Name",
                    CreatorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    CreatedTime = currentTime,
                    LastModifiedTime = currentTime,
                };

                expectedIdentity.HostedAccount = new HostedAccount()
                {
                    Name = "newuser",
                    Password = "newpasswordhash1",
                    CreatorId = expectedIdentity.CreatorId,
                    CreatedTime = expectedIdentity.CreatedTime,
                    LastModifiedTime = expectedIdentity.CreatedTime,
                };

                Add(
                    expectedIdentity,
                    new CreateHostedCommand()
                    {
                        PoolId = expectedIdentity.PoolId,
                        Name = expectedIdentity.Name,
                        AccountName = expectedIdentity.HostedAccount.Name,
                        AccountPassword = "newpassword1",
                        Active = expectedIdentity.Active,
                        RequestorId = expectedIdentity.CreatorId,
                    }
                );

                expectedIdentity = new Identity()
                {
                    PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Active = false,
                    Name = "New User Name",
                    CreatorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    CreatedTime = currentTime,
                    LastModifiedTime = currentTime,
                };

                expectedIdentity.HostedAccount = new HostedAccount()
                {
                    Name = "newuser",
                    Password = "newpasswordhash2",
                    CreatorId = expectedIdentity.CreatorId,
                    CreatedTime = expectedIdentity.CreatedTime,
                    LastModifiedTime = expectedIdentity.CreatedTime,
                };

                Add(
                    expectedIdentity,
                    new CreateHostedCommand()
                    {
                        PoolId = expectedIdentity.PoolId,
                        Name = expectedIdentity.Name,
                        AccountName = expectedIdentity.HostedAccount.Name,
                        AccountPassword = "newpassword2",
                        Active = expectedIdentity.Active,
                        RequestorId = expectedIdentity.CreatorId,
                    }
                );
            }
        }
    }
}
