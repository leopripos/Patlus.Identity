using FluentAssertions;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetAll;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetAll.GetAllQueryValidatorTests
{
    public class Validate_RequestorId_Should_Return_NotEmpty_Error
    {
        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, GetAllQuery query)
        {
            // Arrange
            var validator = new GetAllQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, GetAllQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetAllQuery.RequestorId),
                    new GetAllQuery()
                    {
                        RequestorId = null,
                    }
                );
            }
        }
    }
}
