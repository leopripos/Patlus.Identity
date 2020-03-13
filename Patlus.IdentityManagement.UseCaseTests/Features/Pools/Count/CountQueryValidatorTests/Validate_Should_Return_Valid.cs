using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Pools.Count;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Count.CountQueryValidatorTests
{
    [Trait("UT-Feature", "Pools/Count")]
    [Trait("UT-Class", "Pools/Count/CountQueryValidatorTests")]
    public sealed class Validate_Should_Return_Valid
    {
        [Theory(DisplayName = nameof(Validate_RequestorId_Should_Return_NotEmpty_Error))]
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
