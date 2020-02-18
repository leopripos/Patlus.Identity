using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.CreateHosted.CreateHostedCommandValidatorTests
{
    public class Validate_Name_Should_Return_Pattern_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_Name_Should_Return_Pattern_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateCommand query)
        {
            // Arrange
            var validator = new CreateCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.PatternMatch);
        }

        class TestData : TheoryData<string, CreateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = "*Admin",
                    }
                );

                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = "_Admin",
                    }
                );

                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = "1Admin",
                    }
                );
            }
        }
    }
}
