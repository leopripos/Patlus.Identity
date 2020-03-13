using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/CreateHosted")]
    [Trait("UT-Class", "Identities/CreateHosted/CreateHostedCommandHandlerTests")]
    public sealed class Handle_Should_Throw_NotFoundException : IDisposable
    {
        private readonly IQueryable<Pool> _poolsDataSource;

        private readonly Mock<ILogger<CreateHostedCommandHandler>> _mockLogger;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_NotFoundException()
        {
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Archived = false,
                },
            };
            _poolsDataSource = pools.AsQueryable();

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
        public void Theory(string expectedEntityName, object expectedEntityValue, CreateHostedCommand command)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_poolsDataSource);

            var handler = new CreateHostedCommandHandler(
                _mockLogger.Object,
                _mockMasterDbContext.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
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

        class TestData : TheoryData<string, object, CreateHostedCommand>
        {
            public TestData()
            {
                Add(
                    nameof(Pool),
                    new
                    {
                        PoolId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")
                    },
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                        Name = "New User Name",
                        AccountName = "newuser",
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );
            }
        }
    }
}
