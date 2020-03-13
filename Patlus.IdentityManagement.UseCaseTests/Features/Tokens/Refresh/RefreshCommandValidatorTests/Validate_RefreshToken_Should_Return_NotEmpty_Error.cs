using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Tokens.Refresh.RefreshCommandValidatorTests
{
    [Trait("UT-Feature", "Tokens/Refresh")]
    [Trait("UT-Class", "Tokens/Refresh/RefreshCommandValidatorTests")]
    public sealed class Validate_RefreshToken_Should_Return_NotEmpty_Error
    {
        [Theory(DisplayName = nameof(Validate_RefreshToken_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, RefreshCommand query)
        {
            // Arrange
            var validator = new RefreshCommandValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, RefreshCommand>
        {
            public TestData()
            {
                Add(
                    nameof(RefreshCommand.RefreshToken),
                    new RefreshCommand()
                    {
                        RefreshToken = null,
                    }
                );

                Add(
                    nameof(RefreshCommand.RefreshToken),
                    new RefreshCommand()
                    {
                        RefreshToken = string.Empty,
                    }
                );

                Add(
                    nameof(RefreshCommand.RefreshToken),
                    new RefreshCommand()
                    {
                        RefreshToken = "    ",
                    }
                );
            }
        }
    }
}
