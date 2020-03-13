using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Tokens;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Tokens
{
    [Trait("IT-Feature", "/tokens")]
    [Trait("IT-Feature", "POST /tokens")]
    public sealed class POST_Should_Return_Success_And_Token_Dto : IntegrationTesting
    {
        public POST_Should_Return_Success_And_Token_Dto(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_Success_And_Token_Dto))]
        public async Task Theory()
        {
            // Arrange
            var form = new CreateForm
            {
                PoolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                Name = "root",
                Password = "root"
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await CreateClient().PostAsync("/tokens", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var tokenDto = DeserializeJson<TokenDto>(content);

            (await CreateAutheticatedClient(tokenDto.Scheme, tokenDto.Access).GetAsync("/me/profile")).StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
