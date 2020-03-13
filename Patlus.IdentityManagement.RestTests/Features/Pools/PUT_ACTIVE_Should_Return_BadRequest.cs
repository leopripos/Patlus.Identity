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
    [Trait("IT-Feature", "PUT /pools/{poolId}/active")]
    public sealed class PUT_ACTIVE_Should_Return_BadRequest : IntegrationTesting
    {
        public PUT_ACTIVE_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_ACTIVE_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var form = new UpdateActiveStatusForm
            {
                Active = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PutAsync($"/pools/{poolId}/active", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Message.Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Active));
            errorResult.Details[nameof(form.Active)].Should().NotBeNullOrEmpty();
        }

    }
}
