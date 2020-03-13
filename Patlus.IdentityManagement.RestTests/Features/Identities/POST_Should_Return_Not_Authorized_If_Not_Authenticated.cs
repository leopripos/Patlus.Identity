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
    public sealed class POST_Should_Return_Not_Authorized_If_Not_Authenticated : IntegrationTesting
    {
        public POST_Should_Return_Not_Authorized_If_Not_Authenticated(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_Not_Authorized_If_Not_Authenticated))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
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
            var response = await CreateClient().PostAsync($"/pools/{poolId}/identities", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
