using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Pools;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "PUT /pools/{poolId}/active")]
    public sealed class PUT_ACTIVE_Should_Return_NotFound : IntegrationTesting
    {
        public PUT_ACTIVE_Should_Return_NotFound(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_ACTIVE_Should_Return_NotFound))]
        public async Task Theory()
        {
            // Arrange
            var poolId = Guid.NewGuid();
            var form = new UpdateActiveStatusForm
            {
                Active = false
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PutAsync($"/pools/{poolId}/active", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
