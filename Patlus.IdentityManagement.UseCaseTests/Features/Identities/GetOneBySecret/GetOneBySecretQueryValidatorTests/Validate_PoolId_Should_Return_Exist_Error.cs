using FluentAssertions;
using Moq;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryValidatorTests
{
    public class Validate_PoolId_Should_Return_Exist_Error
    {
        private readonly IQueryable<Pool> _poolsDataSource;

        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_PoolId_Should_Return_Exist_Error()
        {
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Active = false,
                    Archived = false,
                },
                new Pool(){
                    Id = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2"),
                    Active = true,
                    Archived = true,
                },
            };

            _poolsDataSource = pools.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory]
        [ClassData(typeof(TestData))]
        public void Theory(string expectedPropertyName, GetOneBySecretQuery query)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_poolsDataSource);

            var validator = new GetOneBySecretQueryValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .NotBeEmpty()
                .And
                .Contain(e => e.PropertyName == expectedPropertyName && e.ErrorCode == ValidationErrorCodes.Exist);
        }

        class TestData : TheoryData<string, GetOneBySecretQuery>
        {
            public TestData()
            {
                Add(
                    nameof(GetOneBySecretQuery.PoolId),
                    new GetOneBySecretQuery()
                    {
                        PoolId = Guid.NewGuid(),
                    }
                );

                Add(
                    nameof(GetOneBySecretQuery.PoolId),
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    }
                );

                Add(
                    nameof(GetOneBySecretQuery.PoolId),
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("39905588-99d4-4fb5-a41b-18f88c3689d2"),
                    }
                );
            }
        }
    }

}
