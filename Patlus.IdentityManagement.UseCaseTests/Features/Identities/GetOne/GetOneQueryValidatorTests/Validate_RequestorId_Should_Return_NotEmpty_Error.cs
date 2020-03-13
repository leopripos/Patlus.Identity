using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOne.GetOneQueryValidatorTests
{
    [Trait("UT-Feature", "Identities/GetOne")]
    [Trait("UT-Class", "Identities/GetOne/GetOneQueryValidatorTests")]
    public sealed class Validate_RequestorId_Should_Return_NotEmpty_Error
    {
        [Theory(DisplayName = nameof(Validate_RequestorId_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, GetOneQuery query)
        {
            // Arrange
            var validator = new GetOneQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, GetOneQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetOneQuery.RequestorId),
                    new GetOneQuery()
                    {
                        RequestorId = null,
                    }
                );
            }
        }
    }

}
