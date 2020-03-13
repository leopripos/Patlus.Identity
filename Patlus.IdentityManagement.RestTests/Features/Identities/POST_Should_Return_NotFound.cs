using FluentAssertions;
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
    [Trait("IT-Feature", "POST /pools/{poolId}/identities")]
    public sealed class POST_Should_Return_NotFound : IntegrationTesting
    {
        public POST_Should_Return_NotFound(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_NotFound))]
        public async Task Theory()
        {
            // Arrange
            var poolId = Guid.NewGuid();
            var form = new CreateForm
            {
                Name = "New Identity",
                AccountName = "newname",
                AccountPassword = "newpassword"
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PostAsync($"/pools/{poolId}/identities", httpContent);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
