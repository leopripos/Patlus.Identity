using FluentAssertions;
using Patlus.Common.Presentation.Responses.Content;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Identities;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Identities
{
    [Trait("IT-Feature", "/identities")]
    [Trait("IT-Feature", "PUT /pools/{poolId}/identities/{identityId}/active")]
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
            var identityId = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");
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
            var response = await (await CreateAutheticatedClient()).PutAsync($"/pools/{poolId}/identities/{identityId}/active", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Should().NotBeNull();
            errorResult!.Message.Should().NotBeNullOrEmpty();

            errorResult!.Details.Should().NotBeNull();

            errorResult!.Details!.Should().ContainKey(nameof(form.Active));
            errorResult!.Details![nameof(form.Active)].Should().NotBeNullOrEmpty();
        }

    }
}
