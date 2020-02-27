using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandValidatorTests
{
    [Trait("UT-Feature", "Identities/CreateHosted")]
    [Trait("UT-Class", "Identities/CreateHosted/CreateHostedCommandValidatorTests")]
    public class Validate_AccountPassword_Should_Return_NotEmpty_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_AccountPassword_Should_Return_NotEmpty_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_AccountPassword_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateHostedCommand query)
        {
            // Arrange
            var validator = new CreateHostedCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty().And.Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, CreateHostedCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateHostedCommand.AccountPassword),
                    new CreateHostedCommand()
                    {
                        AccountPassword = null,
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountPassword),
                    new CreateHostedCommand()
                    {
                        AccountPassword = string.Empty,
                    }
                );

                Add(
                    nameof(CreateHostedCommand.AccountPassword),
                    new CreateHostedCommand()
                    {
                        AccountPassword = "   ",
                    }
                );
            }
        }
    }
}
