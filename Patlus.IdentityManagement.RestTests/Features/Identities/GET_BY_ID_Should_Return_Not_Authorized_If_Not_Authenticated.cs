using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Identities
{
    [Trait("IT-Feature", "/identities")]
    [Trait("IT-Feature", "GET /pools/{poolId}/identities/{identityId}")]
    public sealed class GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated : IntegrationTesting
    {
        public GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_BY_ID_Should_Return_Not_Authorized_If_Not_Authenticated))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var identityId = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");

            // Act
            var response = await CreateClient().GetAsync($"/pools/{poolId}/identities/{identityId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
