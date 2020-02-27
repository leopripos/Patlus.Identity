using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Create.CreateCommandValidatorTests
{
    [Trait("UT-Feature", "Pools/Create")]
    [Trait("UT-Class", "Pools/Create/CreateCommandValidatorTests")]
    public class Validate_Description_Should_Return_NotEmpty_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_Description_Should_Return_NotEmpty_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_Description_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateCommand query)
        {
            // Arrange
            var validator = new CreateCommandValidator(this._mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, CreateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateCommand.Description),
                    new CreateCommand()
                    {
                        Description = null,
                    }
                );

                Add(
                    nameof(CreateCommand.Description),
                    new CreateCommand()
                    {
                        Description = string.Empty,
                    }
                );

                Add(
                    nameof(CreateCommand.Description),
                    new CreateCommand()
                    {
                        Description = "      ",
                    }
                );
            }
        }
    }
}
