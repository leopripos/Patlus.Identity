using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Me;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Me
{
    [Trait("IT-Feature", "/me")]
    [Trait("IT-Feature", "PUT /me/password")]
    public sealed class PUT_PASSWORD_Should_Return_Not_Authorized_If_Not_Authenticated : IntegrationTesting
    {
        public PUT_PASSWORD_Should_Return_Not_Authorized_If_Not_Authenticated(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(PUT_PASSWORD_Should_Return_Not_Authorized_If_Not_Authenticated))]
        public async Task Theory()
        {
            // Arrange
            var form = new UpdatePasswordForm
            {
                OldPassword = "root",
                NewPassword = "newpassword",
                RetypeNewPassword = "newpassword",
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await CreateClient().PutAsync("/me/password", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
