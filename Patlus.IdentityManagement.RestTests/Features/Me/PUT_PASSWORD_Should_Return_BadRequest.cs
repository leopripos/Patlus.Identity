using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Me;
using Patlus.IdentityManagement.Rest.Responses.Content;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Me
{
    [Trait("IT-Feature", "/me")]
    [Trait("IT-Feature", "PUT /me/password")]
    public sealed class PUT_PASSWORD_Should_Return_BadRequest : IntegrationTesting
    {
        public PUT_PASSWORD_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_PASSWORD_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var client = await CreateAutheticatedClient();
            var form = new UpdatePasswordForm
            {
                OldPassword = null,
                NewPassword = null,
                RetypeNewPassword = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await client.PutAsync("/me/password", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Message.Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.OldPassword));
            errorResult.Details[nameof(form.OldPassword)].Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.NewPassword));
            errorResult.Details[nameof(form.NewPassword)].Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.RetypeNewPassword));
            errorResult.Details[nameof(form.RetypeNewPassword)].Should().NotBeNullOrEmpty();
        }
    }
}
