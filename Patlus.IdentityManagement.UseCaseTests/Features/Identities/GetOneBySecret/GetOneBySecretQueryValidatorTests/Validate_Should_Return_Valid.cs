using FluentAssertions;
using Moq;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Patlus.IdentityManagement.UseCaseTests.Features.Identities.GetOneBySecret.GetOneBySecretQueryValidatorTests
{
    [Trait("UT-Feature", "Identities/GetOneBySecret")]
    [Trait("UT-Class", "Identities/GetOneBySecret/GetOneBySecretQueryValidatorTests")]
    public class Validate_Should_Return_Valid
    {
        private readonly IQueryable<Pool> _poolsDataSource;

        private readonly Mock<IMasterDbContext> _mockMasterDbContext;

        public Validate_Should_Return_Valid()
        {
            var pools = new List<Pool>() {
                new Pool(){
                    Id = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                    Active = true,
                    Archived = false,
                },
            };

            _poolsDataSource = pools.AsQueryable();
            _mockMasterDbContext = new Mock<IMasterDbContext>();
        }

        [Theory(DisplayName = nameof(Validate_Should_Return_Valid))]
        [ClassData(typeof(TestData))]
        public void Theory(GetOneBySecretQuery query)
        {
            // Arrange
            _mockMasterDbContext.SetupGet(e => e.Pools).Returns(_poolsDataSource);

            var validator = new GetOneBySecretQueryValidator(_mockMasterDbContext.Object);

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        class TestData : TheoryData<GetOneBySecretQuery>
        {
            public TestData()
            {
                Add(
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "1",
                        Password = "1",
                        RequestorId = null,
                    }
                );

                Add(
                    new GetOneBySecretQuery()
                    {
                        PoolId = new Guid("821e7913-876f-4377-a799-17fb8b5a0a49"),
                        Name = "leopripos",
                        Password = "leopripos",
                        RequestorId = null,
                    }
                );
            }
        }
    }

}
