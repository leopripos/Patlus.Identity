using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetOne;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetOne.GetOneQueryValidatorTests
{
    [Trait("UT-Feature", "Pools/GetOne")]
    [Trait("UT-Class", "Pools/GetOne/GetOneQueryValidatorTests")]
    public class Validate_Condition_Should_Return_NotEmpty_Error
    {
        [Theory(DisplayName = nameof(Validate_Condition_Should_Return_NotEmpty_Error))]
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
                    nameof(GetOneQuery.Condition),
                    new GetOneQuery()
                    {
                        Condition = null
                    }
                );
            }
        }
    }

}
