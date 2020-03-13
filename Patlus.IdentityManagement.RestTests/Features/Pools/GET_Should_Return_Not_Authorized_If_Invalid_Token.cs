using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "GET /pools")]
    public sealed class GET_Should_Return_Not_Authorized_If_Invalid_Token : IntegrationTesting
    {
        public GET_Should_Return_Not_Authorized_If_Invalid_Token(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_Should_Return_Not_Authorized_If_Invalid_Token))]
        public async Task Theory()
        {
            // Act
            var response = await CreateAutheticatedClient("Bearer", "invalidtoken").GetAsync("/pools");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
