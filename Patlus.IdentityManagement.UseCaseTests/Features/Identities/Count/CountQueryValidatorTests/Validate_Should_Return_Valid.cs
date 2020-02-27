using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Identities.Count;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.Count.CountQueryValidatorTests
{
    [Trait("UT-Feature", "Identities/Count")]
    [Trait("UT-Class", "Identities/Count/CountQueryValidatorTests")]
    public class Validate_Should_Return_Valid
    {
        [Theory(DisplayName = nameof(Validate_Should_Return_Valid))]
        [ClassData(typeof(TestData))]
        public void Theory(CountQuery query)
        {
            // Arrange
            var validator = new CountQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<CountQuery>
        {
            public TestData()
            {
                Add(
                    new CountQuery()
                    {
                        RequestorId = Guid.NewGuid(),
                    }
                );
            }
        }
    }
}
