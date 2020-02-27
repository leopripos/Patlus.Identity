using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.CreateHosted.CreateHostedCommandValidatorTests
{
    [Trait("UT-Feature", "Identities/CreateHosted")]
    [Trait("UT-Class", "Identities/CreateHosted/CreateHostedCommandValidatorTests")]
    public class Validate_PoolId_Should_Return_Exist_Error
    {
        private readonly IQueryable<Pool> _dataSource;

        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_PoolId_Should_Return_Exist_Error()
        {
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Archived = true,
                },
            };

            _dataSource = pools.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_PoolId_Should_Return_Exist_Error))]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, CreateHostedCommand query)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_dataSource);

            var validator = new CreateHostedCommandValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.Exist);
        }

        class TestData : TheoryData<string, CreateHostedCommand>
        {
            public TestData()
            {
                Add(
                    nameof(CreateHostedCommand.PoolId),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    }
                );

                Add(
                    nameof(CreateHostedCommand.PoolId),
                    new CreateHostedCommand()
                    {
                        PoolId = new Guid("29899885-bbf1-430f-b9d6-613066b4021a"),
                    }
                );
            }
        }
    }
}
