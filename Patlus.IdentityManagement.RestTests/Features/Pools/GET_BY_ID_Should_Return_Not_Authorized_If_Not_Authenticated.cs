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
    public sealed class GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated : IntegrationTesting
    {
        public GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated))]
        public async Task Theory()
        {
            // Act
            var response = await CreateClient().GetAsync($"/pools/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
