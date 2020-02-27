using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryValidatorTests
{
    [Trait("UT-Feature", "Identities/GetOneBySecret")]
    [Trait("UT-Class", "Identities/GetOneBySecret/GetOneBySecretQueryValidatorTests")]
    public class Validate_PoolId_Should_Return_NotEmpty_Error
    {
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_PoolId_Should_Return_NotEmpty_Error()
        {
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_PoolId_Should_Return_NotEmpty_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, GetOneBySecretQuery query)
        {
            // Arrange
            var validator = new GetOneBySecretQueryValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.NotEmpty);
        }

        class TestData : TheoryData<string, GetOneBySecretQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetOneBySecretQuery.PoolId),
                    new GetOneBySecretQuery()
                    {
                        PoolId = null,
                    }
                );
            }
        }
    }

}
