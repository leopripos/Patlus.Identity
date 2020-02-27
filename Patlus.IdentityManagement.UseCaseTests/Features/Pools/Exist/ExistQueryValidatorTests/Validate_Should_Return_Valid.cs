using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Pools.Exist;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Exist.ExistQueryValidatorTests
{
    [Trait("UT-Feature", "Pools/Exist")]
    [Trait("UT-Class", "Pools/Exist/ExistQueryValidatorTests")]
    public class Validate_Should_Return_Valid
    {
        [Theory(DisplayName = nameof(Validate_Should_Return_Valid))]
        [ClassData(typeof(TestData))]
        public void Theory(ExistQuery query)
        {
            // Arrange
            var validator = new ExistQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<ExistQuery>
        {
            public TestData()
            {
                Add(
                    new ExistQuery()
                    {
                        Condition = e => true,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    new ExistQuery()
                    {
                        Condition = e => e.Active == true,
                        RequestorId = Guid.NewGuid(),
                    }
                );
            }
        }
    }
}
