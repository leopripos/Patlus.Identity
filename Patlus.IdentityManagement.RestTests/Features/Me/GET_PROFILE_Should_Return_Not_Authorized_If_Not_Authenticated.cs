using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Me
{
    [Trait("IT-Feature", "/me")]
    [Trait("IT-Feature", "GET /me/profile")]
    public sealed class GET_PROFILE_Should_Return_Not_Authorized_If_Not_Authenticated : IntegrationTesting
    {
        public GET_PROFILE_Should_Return_Not_Authorized_If_Not_Authenticated(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_PROFILE_Should_Return_Not_Authorized_If_Not_Authenticated))]
        public async Task Theory()
        {
            // Act
            var response = await CreateClient().GetAsync("/me/profile");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
