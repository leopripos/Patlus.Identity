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
    public partial class GET_BY_ID_Should_Return_Not_Authorized_If_Invalid_Token : IntegrationTesting
    {
        public GET_BY_ID_Should_Return_Not_Authorized_If_Invalid_Token(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_BY_ID_Should_Return_Not_Authorized_If_Invalid_Token))]
        public async Task Theory()
        {
            // Act
            var response = await CreateAutheticatedClient("Bearer", "invalidtoken").GetAsync($"/pools/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
