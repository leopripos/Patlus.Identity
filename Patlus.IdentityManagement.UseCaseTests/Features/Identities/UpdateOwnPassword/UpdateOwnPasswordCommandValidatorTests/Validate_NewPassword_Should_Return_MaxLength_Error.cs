using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateOwnPassword.UpdateOwnPasswordCommandValidatorTests
{
    public class Validate_NewPassword_Should_Return_MaxLength_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;
        private readonly Mock<IPasswordService> _mockPasswordService;


        public Validate_NewPassword_Should_Return_MaxLength_Error()
        {
            this._mockMasterDbContext = new Mock<IMasterDbContext>();
            this._mockPasswordService = new Mock<IPasswordService>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, UpdateOwnPasswordCommand command)
        {
            // Arrange
            var validator = new UpdateOwnPasswordCommandValidator(
                dbService: this._mockMasterDbContext.Object,
                passwordService: this._mockPasswordService.Object
            );

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.MaxLength);
        }

        class TestData : TheoryData<string, UpdateOwnPasswordCommand>
        {
            public TestData()
            {
                Add(
                    nameof(UpdateOwnPasswordCommand.NewPassword),
                    new UpdateOwnPasswordCommand()
                    {
                        NewPassword = "123456789012345678901",
                    }
                );
            }
        }
    }

}
