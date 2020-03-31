using FluentAssertions;
using Patlus.Common.Presentation.Responses.Errors;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Tokens;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Tokens
{
    [Trait("IT-Feature", "/tokens")]
    [Trait("IT-Feature", "POST /tokens")]
    public sealed class POST_Should_Return_BadRequest : IntegrationTesting
    {
        public POST_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var form = new CreateForm
            {
                PoolId = null,
                Name = null,
                Password = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await CreateClient().PostAsync("/tokens", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Should().NotBeNull();
            errorResult!.Message.Should().NotBeNullOrEmpty();

            errorResult!.Details.Should().NotBeNull();

            errorResult!.Details!.Should().ContainKey(nameof(form.PoolId));
            errorResult!.Details![nameof(form.PoolId)].Should().NotBeNullOrEmpty();

            errorResult!.Details!.Should().ContainKey(nameof(form.Name));
            errorResult!.Details![nameof(form.Name)].Should().NotBeNullOrEmpty();

            errorResult!.Details!.Should().ContainKey(nameof(form.Password));
            errorResult!.Details![nameof(form.Password)].Should().NotBeNullOrEmpty();
        }
    }
}
