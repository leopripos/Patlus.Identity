using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOne.GetOneQueryValidatorTests
{
    public class Validate_Should_Return_Valid
    {
        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(GetOneQuery query)
        {
            // Arrange
            var validator = new GetOneQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<GetOneQuery>
        {
            public TestData()
            {
                Add(
                    new GetOneQuery()
                    {
                        Condition = e => true,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    new GetOneQuery()
                    {
                        Condition = e => e.Active == true,
                        RequestorId = Guid.NewGuid(),
                    }
                );
            }
        }
    }
}
