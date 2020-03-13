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
    [Trait("IT-Feature", "PATCH /pools/{poolId}")]
    public sealed class PATCH_Should_Return_NotFound : IntegrationTesting
    {
        public PATCH_Should_Return_NotFound(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PATCH_Should_Return_NotFound))]
        public async Task Theory()
        {
            // Arrange
            var poolId = Guid.NewGuid();
            var form = new UpdateForm
            {
                Name = "New Name",
                Description = "New Description"
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PatchAsync($"/pools/{poolId}", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
