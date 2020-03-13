using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "GET /pools/{poolId}")]
    public partial class GET_BY_ID_Should_Return_Not_Found : IntegrationTesting
    {
        public GET_BY_ID_Should_Return_Not_Found(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_BY_ID_Should_Return_Not_Found))]
        public async Task Theory()
        {
            // Arrange
            var client = await CreateAutheticatedClient();

            // Act
            var response = await client.GetAsync($"/pools/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
