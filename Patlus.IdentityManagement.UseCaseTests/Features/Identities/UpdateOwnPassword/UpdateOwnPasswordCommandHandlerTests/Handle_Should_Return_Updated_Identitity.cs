using FluentAssertions;
using MediatR;
using Moq;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateOwnPassword.UpdateOwnPasswordCommandHandlerTests
{
    [Trait("UT-Feature", "Identities/UpdateOwnPassword")]
    [Trait("UT-Class", "Identities/UpdateOwnPassword/UpdateOwnPasswordCommandHandlerTests")]
    public sealed class Handle_Should_Return_Updated_Identitity : IDisposable
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IIdentifierService> _mockIdentifierService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IPasswordService> _mockPasswordService;

        public Handle_Should_Return_Updated_Identitity()
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

        [Theory(DisplayName = nameof(Handle_Should_Return_Updated_Identitity))]
        [ClassData(typeof(TestData))]
        public async void Theory(Identity previousValue, UpdateOwnPasswordCommand command)
        {
            // Arrange
            var currentTime = DateTimeOffset.Now;
            var dataSource = IdentitiesFaker.CreateIdentities();
            _mockMasterDbContext.Setup(e => e.Identities).Returns(dataSource);
            _mockTimeService.Setup(e => e.Now).Returns(currentTime);
            _mockPasswordService.Setup(e => e.GeneratePasswordHash("newpassword")).Returns("newpasswordhash");
            _mockPasswordService.Setup(e => e.ValidatePasswordHash(It.IsAny<string>(), "rightpassword")).Returns(true);

            var handler = new UpdateOwnPasswordCommandHandler(
                _mockMasterDbContext.Object,
                _mockIdentifierService.Object,
                _mockTimeService.Object,
                _mockMediator.Object,
                _mockPasswordService.Object
            );

            // Act
            var actualResult = await handler.Handle(command, default);

            // Assert
            actualResult.Should().BeEquivalentTo(previousValue, opt =>
            {
                opt = opt.IgnoringCyclicReferences();
                opt = opt.Excluding(e => e.AuthKey);
                opt = opt.Excluding(e => e.LastModifiedTime);
                opt = opt.Excluding(e => e.HostedAccount!.Password);
                opt = opt.Excluding(e => e.HostedAccount!.LastModifiedTime);

                return opt;
            });

            actualResult.AuthKey.Should().NotBe(previousValue.AuthKey);
            actualResult.LastModifiedTime.Should().Be(currentTime);
            actualResult.HostedAccount!.Password.Should().Be("newpasswordhash");
            actualResult.HostedAccount!.LastModifiedTime.Should().Be(currentTime);

            _mockMediator.Verify(
                e => e.Publish(
                    It.Is<OwnPasswordUdpatedNotification>(notif => (
                        notif.Entity == actualResult
                    )),
                    It.IsAny<CancellationToken>()
                ), Times.Once);
        }

        class TestData : TheoryData<Identity?, UpdateOwnPasswordCommand>
        {
            public TestData()
            {
                var dataSource = IdentitiesFaker.CreateIdentities();
                Add(
                    dataSource.Where(e => (
                        e.Id == new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605")
                        && e.Archived == false
                    )).FirstOrDefault(),
                    new UpdateOwnPasswordCommand()
                    {
                        OldPassword = "rightpassword",
                        NewPassword = "newpassword",
                        RetypeNewPassword = "newpassword",
                        RequestorId = new Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605")
                    }
                );
            }
        }
    }
}
