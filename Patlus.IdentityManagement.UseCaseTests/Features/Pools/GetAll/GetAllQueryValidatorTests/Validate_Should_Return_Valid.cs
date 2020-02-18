using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Pools.GetAll;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetAll.GetAllQueryValidatorTests
{
    public class Validate_Should_Return_Valid
    {
        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(GetAllQuery query)
        {
            // Arrange
            var validator = new GetAllQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<GetAllQuery>
        {
            public TestData()
            {
                Add(
                    new GetAllQuery()
                    {
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    new GetAllQuery()
                    {
                        Condition = null,
                        RequestorId = Guid.NewGuid(),
                    }
                );

                Add(
                    new GetAllQuery()
                    {
                        Condition = e => e.Active == true,
                        RequestorId = Guid.NewGuid(),
                    }
                );
            }
        }
    }
}
