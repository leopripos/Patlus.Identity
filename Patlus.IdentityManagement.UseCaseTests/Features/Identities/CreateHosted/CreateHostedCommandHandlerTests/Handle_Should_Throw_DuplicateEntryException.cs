using FluentAssertions;
using MediatR;
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
    public sealed class Handle_Should_Throw_DuplicateEntryException : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IIdentifierService> _mockIdentifierService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Throw_DuplicateEntryException()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockIdentifierService = new Mock<IIdentifierService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockMediator = new Mock<IMediator>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        public void Dispose()
        {
            _mockMasterDbContext.Reset();
            _mockIdentifierService.Reset();
            _mockTimeService.Reset();
            _mockMediator.Reset();
            _mockPasswordService.Reset();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedEntityName, object expectedEntityValue, CreateHostedCommand command)
        {
            // Arrange
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Archived = false,
                },
            };

            var hostedAccounts = new List<HostedAccount>() {
                new HostedAccount()
                {
                    Id =  Guid.NewGuid(),
                    Name = "leopripos"
                }
            };

            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(pools.AsQueryable());
            _mockMasterDbContext.SetupGet(e => e.HostedAccounts).Returns(hostedAccounts.AsQueryable());

            var handler = new CreateHostedCommandHandler(
                _mockMasterDbContext.Object,
                _mockIdentifierService.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
            );

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            action.Should().Throw<DuplicateEntryException>().Where(e => (
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
                    nameof(HostedAccount),
                    new
                    {
                        AccountName = "leopripos"
                    },
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "Leo Pripos Marbun",
                        AccountName = "leopripos",
                        AccountPassword = "newpassword2",
                        Active = true,
                        RequestorId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    }
                );
            }
        }
    }
}
