using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Update.UpdateCommandValidatorTests
{
    [Trait("UT-Feature", "Pools/Update")]
    [Trait("UT-Class", "Pools/Update/UpdateCommandValidatorTests")]
    public sealed class Validate_RequestorId_Should_Return_NotEmpty_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_RequestorId_Should_Return_NotEmpty_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_RequestorId_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, UpdateCommand command)
        {
            // Arrange
            var validator = new UpdateCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, UpdateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(UpdateCommand.RequestorId),
                    new UpdateCommand()
                    {
                        RequestorId = null,
                    }
                );
            }
        }
    }

}
