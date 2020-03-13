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
    public sealed class PATCH_Should_Return_Success_And_Updated_Pool : IntegrationTesting
    {
        public PATCH_Should_Return_Success_And_Updated_Pool(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PATCH_Should_Return_Success_And_Updated_Pool))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
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
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var pool = DeserializeJson<PoolDto>(content);

            pool.Name.Should().Be(form.Name);
            pool.Description.Should().Be(form.Description);
        }

    }
}
