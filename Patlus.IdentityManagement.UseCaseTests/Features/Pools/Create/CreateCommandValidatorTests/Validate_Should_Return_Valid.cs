using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.GetOne.CreateCommandValidatorTests
{
    public class Validate_Should_Return_Valid
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_Should_Return_Valid()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(CreateCommand command)
        {
            // Arrange
            var validator = new CreateCommandValidator(this._mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<CreateCommand>
        {
            public TestData()
            {
                Add(
                    new CreateCommand()
                    {
                        Name = "Development Vendor",
                        Active = true,
                        Description = "This pool is used to pool vendor users.",
                        RequestorId = Guid.NewGuid(),
                    }
                );
            }
        }
    }
}
