using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Pools;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "GET /pools/{poolId}")]
    public sealed class GET_BY_ID_Should_Return_Success_And_Pool : IntegrationTesting
    {
        public GET_BY_ID_Should_Return_Success_And_Pool(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_BY_ID_Should_Return_Success_And_Pool))]
        public async Task Theory()
        {
            // Arrange
            var client = await CreateAutheticatedClient();
            var expectedResult = new PoolDto()
            {
                Id = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                Active = true,
                Name = "Administrator Pool",
                Description = "Default identity pool for system administrator.",
                CreatorId = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                CreatedTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1))
            };

            // Act
            var response = await client.GetAsync($"/pools/{expectedResult.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var pool = DeserializeJson<PoolDto>(content);

            pool.Should().BeEquivalentTo(expectedResult);
        }
    }
}
