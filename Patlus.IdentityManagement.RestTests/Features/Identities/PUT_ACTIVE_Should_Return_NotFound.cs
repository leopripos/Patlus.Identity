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
    [Trait("IT-Feature", "PUT /pools/{poolId}/identities/{identityId}/active")]
    public sealed class PUT_ACTIVE_Should_Return_NotFound : IntegrationTesting
    {
        public PUT_ACTIVE_Should_Return_NotFound(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Theory(DisplayName = nameof(PUT_ACTIVE_Should_Return_NotFound))]
        [ClassData(typeof(TestData))]
        public async Task Theory(Guid poolId, Guid identityId)
        {
            // Arrange
            var form = new UpdateActiveStatusForm
            {
                Active = false
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PutAsync($"/pools/{poolId}/identiteis/{identityId}/active", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        class TestData : TheoryData<Guid, Guid>
        {
            public TestData()
            {
                Add(
                    Guid.NewGuid(),
                    new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026")
                );

                Add(
                    new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                    Guid.NewGuid()
                );
            }
        }

    }
}
