using FluentAssertions;
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
    [Trait("IT-Feature", "PUT /tokens")]
    public sealed class PUT_Should_Return_Success_And_Token_Dto : IntegrationTesting
    {
        public PUT_Should_Return_Success_And_Token_Dto(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_Should_Return_Success_And_Token_Dto))]
        public async Task Theory()
        {
            // Arrange
            var token = await CreateToken();

            var form = new RefreshForm
            {
                RefreshToken = token.Refresh
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await CreateClient().PutAsync("/tokens", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var tokenDto = DeserializeJson<TokenDto>(content);

            (await CreateAutheticatedClient(tokenDto.Scheme, tokenDto.Access).GetAsync("/me/profile")).StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
