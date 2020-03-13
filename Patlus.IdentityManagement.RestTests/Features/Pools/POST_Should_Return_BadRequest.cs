﻿using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Pools;
using Patlus.IdentityManagement.Rest.Responses.Content;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "POST /pools")]
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
                Name = null,
                Description = null,
                Active =  null
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PostAsync("/pools", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var errorResult = DeserializeJson<ValidationErrorDto>(content);

            errorResult.Message.Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Name));
            errorResult.Details[nameof(form.Name)].Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Description));
            errorResult.Details[nameof(form.Description)].Should().NotBeNullOrEmpty();

            errorResult.Details.Should().ContainKey(nameof(form.Active));
            errorResult.Details[nameof(form.Active)].Should().NotBeNullOrEmpty();
        }

    }
}
