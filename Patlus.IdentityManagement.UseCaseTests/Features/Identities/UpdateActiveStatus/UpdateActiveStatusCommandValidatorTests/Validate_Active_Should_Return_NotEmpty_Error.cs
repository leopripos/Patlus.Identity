using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.UpdateActiveStatus.UpdateActiveStatusCommandValidatorTests
{
    [Trait("UT-Feature", "Identities/UpdateActiveStatus")]
    [Trait("UT-Class", "Identities/UpdateActiveStatus/UpdateActiveStatusCommandValidatorTests")]
    public class Validate_Active_Should_Return_NotEmpty_Error
    {
        [Theory(DisplayName = nameof(Validate_Active_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, UpdateActiveStatusCommand command)
        {
            // Arrange
            var validator = new UpdateActiveStatusCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, UpdateActiveStatusCommand>
        {
            public TestData()
            {
                Add(
                    nameof(UpdateActiveStatusCommand.Active),
                    new UpdateActiveStatusCommand()
                    {
                        Active = null,
                    }
                );
            }
        }
    }

}
