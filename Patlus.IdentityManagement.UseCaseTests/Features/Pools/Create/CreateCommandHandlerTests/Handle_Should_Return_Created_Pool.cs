﻿using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.CreateHosted.CreateCommandHandlerTests
{
    public class Handle_Should_Return_Created_Pool : IDisposable
    {
        private readonly Mock<ILogger<CreateCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;

        public Handle_Should_Return_Created_Pool()
        {
            _mockLogger = new Mock<ILogger<CreateCommandHandler>>();
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
        public async void Theory(Pool expectedResult, CreateCommand query)
        {
            // Arrange
            _mockTimeService.SetupGet(e => e.Now).Returns(new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1)));

            var handler = new CreateCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object
            );

            // Act
            var actualResult = await handler.Handle(query, default);

            //Asert
            actualResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.IgnoringCyclicReferences();
                options.Excluding(e => e.Id);

                return options;
            });

            _mockMasterDbContext.Verify(e => e.Add(It.Is<Pool>(e => e == actualResult)), Times.Once);
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

        class TestData : TheoryData<Pool, CreateCommand>
        {
            public TestData()
            {
                var currentTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1));
                Pool expectedIdentity;

                expectedIdentity = new Pool()
                {
                    Name = "Development Vendor",
                    Description = "Development Vendor description",
                    CreatorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    CreatedTime = currentTime,
                    LastModifiedTime = currentTime,
                    Active = true,
                    Archived = false,
                };

                Add(
                    expectedIdentity,
                    new CreateCommand()
                    {
                        Name = expectedIdentity.Name,
                        Description = expectedIdentity.Description,
                        Active = expectedIdentity.Active,
                        RequestorId = expectedIdentity.CreatorId
                    }
                );
            }
        }
    }
}
