using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Pools;
using Patlus.IdentityManagement.Rest.Responses.Content;
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
    public sealed class PATCH_Should_Return_BadRequest : IntegrationTesting
    {
        public PATCH_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PATCH_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var form = new UpdateForm
            {
                Name = null,
                Description = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PatchAsync($"/pools/{poolId}", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Message.Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Name));
            errorResult.Details[nameof(form.Name)].Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Description));
            errorResult.Details[nameof(form.Description)].Should().NotBeNullOrEmpty();
        }

    }
}
