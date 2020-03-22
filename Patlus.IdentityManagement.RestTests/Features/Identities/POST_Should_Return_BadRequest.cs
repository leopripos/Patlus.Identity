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
    [Trait("IT-Feature", "POST /pools/{poolId}/identities")]
    public sealed class POST_Should_Return_BadRequest : IntegrationTesting
    {
        public POST_Should_Return_BadRequest(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_BadRequest))]
        public async Task Theory()
        {
            // Arrange
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var form = new CreateForm
            {
                Name = null,
                AccountName = null,
                AccountPassword = null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PostAsync($"/pools/{poolId}/identities", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Should().NotBeNull();
            errorResult!.Message.Should().NotBeNullOrEmpty();

            errorResult!.Details.Should().NotBeNull();

            errorResult!.Details!.Should().ContainKey(nameof(form.Name));
            errorResult!.Details![nameof(form.Name)].Should().NotBeNullOrEmpty();

            errorResult!.Details!.Should().ContainKey(nameof(form.AccountName));
            errorResult!.Details![nameof(form.AccountName)].Should().NotBeNullOrEmpty();

            errorResult!.Details!.Should().ContainKey(nameof(form.AccountPassword));
            errorResult!.Details![nameof(form.AccountPassword)].Should().NotBeNullOrEmpty();
        }

    }
}
