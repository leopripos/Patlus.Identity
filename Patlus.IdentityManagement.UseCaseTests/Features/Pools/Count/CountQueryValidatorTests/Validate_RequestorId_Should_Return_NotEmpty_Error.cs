using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.Count;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Count.CountQueryValidatorTests
{
    [Trait("UT-Feature", "Pools/Count")]
    [Trait("UT-Class", "Pools/Count/CountQueryValidatorTests")]
    public sealed class Validate_RequestorId_Should_Return_NotEmpty_Error
    {
        [Theory(DisplayName = nameof(Validate_RequestorId_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CountQuery query)
        {
            // Arrange
            var validator = new CountQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, CountQuery>
        {
            public TestData()
            {
                Add(
                    nameof(CountQuery.RequestorId),
                    new CountQuery()
                    {
                        RequestorId = null,
                    }
                );
            }
        }
    }
}
