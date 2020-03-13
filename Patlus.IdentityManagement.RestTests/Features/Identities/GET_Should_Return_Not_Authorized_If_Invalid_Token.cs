using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Identities
{
    [Trait("IT-Feature", "/identities")]
    [Trait("IT-Feature", "GET /pools/{poolId}/identities")]
    public sealed class GET_Should_Return_Not_Authorized_If_Invalid_Token : IntegrationTesting
    {
        public GET_Should_Return_Not_Authorized_If_Invalid_Token(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_Should_Return_Not_Authorized_If_Invalid_Token))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");

            // Act
            var response = await CreateAutheticatedClient("Bearer", "invalidtoken").GetAsync($"/pools/{poolId}/identities");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
