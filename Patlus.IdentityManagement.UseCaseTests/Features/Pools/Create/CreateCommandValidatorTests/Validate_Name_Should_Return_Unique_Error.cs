using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Pools.Create.CreateCommandValidatorTests
{
    [Trait("UT-Feature", "Pools/Create")]
    [Trait("UT-Class", "Pools/Create/CreateCommandValidatorTests")]
    public class Validate_Name_Should_Return_Unique_Error
    {
        private readonly IQueryable<Pool> _poolsDataSource;
        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_Name_Should_Return_Unique_Error()
        {
            _poolsDataSource = PoolsFaker.CreatePools().Values.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_Name_Should_Return_Unique_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateCommand query)
        {
            // Arrange
            _mockMasterDbContext.Setup(e => e.Pools).Returns(_poolsDataSource);

            var validator = new CreateCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.Unique);
        }

        class TestData : TheoryData<string, CreateCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = "System Administrator",
                    }
                );

                Add(
                    nameof(CreateCommand.Name),
                    new CreateCommand()
                    {
                        Name = "Employee",
                    }
                );
            }
        }
    }
}
