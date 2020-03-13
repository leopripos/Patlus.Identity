using FluentAssertions;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetAll;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetAll.GetAllQueryValidatorTests
{
    [Trait("UT-Feature", "Identities/GetAll")]
    [Trait("UT-Class", "Identities/GetAll/GetAllQueryValidatorTests")]
    public sealed class Validate_Should_Return_Valid
    {
        [Theory(DisplayName = nameof(Validate_Should_Return_Valid))]
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
