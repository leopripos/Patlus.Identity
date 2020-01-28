using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateOwnPassword.UpdateOwnPasswordCommandValidatorTests
{
    public class Validate_OldPassword_Should_Return_Invalid_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;


        public Validate_OldPassword_Should_Return_Invalid_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
            _mockPasswordService = new Mock<IPasswordService>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, UpdateOwnPasswordCommand command)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Identities).Returns(IdentitiesFaker.CreateIdentities().Values.AsQueryable());
            _mockPasswordService.Setup(e => e.ValidatePasswordHash("sysadminpassword0", "rightpassword")).Returns(true);
            _mockPasswordService.Setup(e => e.ValidatePasswordHash("sysadminpassword0", "wrongpassword")).Returns(false);

            var validator = new UpdateOwnPasswordCommandValidator(
                dbService: _mockMasterDbContext.Object,
                passwordService: _mockPasswordService.Object
            );

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.Invalid);
        }

        class TestData : TheoryData<string, UpdateOwnPasswordCommand>
        {
            public TestData()
            {
                Add(
                    nameof(UpdateOwnPasswordCommand.OldPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        RequestorId = new System.Guid("9b76c5e9-fe62-4598-ba99-16ca96e5c605"),
                        OldPassword = "wrongpassword",
                    }
                );

                Add(
                    nameof(UpdateOwnPasswordCommand.OldPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        RequestorId = new System.Guid("821e7913-876f-4377-a799-17fb8b5a0a49"), // Invalid id
                        OldPassword = "rightpassword",
                    }
                );
            }
        }
    }

}
