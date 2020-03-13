using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Tokens;
using Patlus.IdentityManagement.Rest.Responses.Content;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Tokens
{
    [Trait("IT-Feature", "/tokens")]
    [Trait("IT-Feature", "PUT /tokens")]
    public sealed class PUT_Should_Return_BadRequest : IntegrationTesting
    {
        public PUT_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var form = new RefreshForm
            {
                RefreshToken = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await CreateClient().PutAsync("/tokens", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Message.Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.RefreshToken));
            errorResult.Details[nameof(form.RefreshToken)].Should().NotBeNullOrEmpty();
        }
    }
}
