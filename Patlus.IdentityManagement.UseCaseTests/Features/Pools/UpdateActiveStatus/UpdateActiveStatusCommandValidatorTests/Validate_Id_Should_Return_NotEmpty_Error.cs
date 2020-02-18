using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetOne.UpdateActiveStatusCommandValidatorTests
{
    public class Validate_Id_Should_Return_NotEmpty_Error
    {
        [Theory]
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
                    nameof(UpdateActiveStatusCommand.Id),
                    new UpdateActiveStatusCommand()
                    {
                        Id = null,
                    }
                );
            }
        }
    }

}
