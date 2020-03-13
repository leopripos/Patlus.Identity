using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Identities;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Identities
{
    [Trait("IT-Feature", "/identities")]
    [Trait("IT-Feature", "GET /pools/{poolId}/identities")]
    public sealed class GET_Should_Return_Success_And_Identities_Array : IntegrationTesting
    {
        public GET_Should_Return_Success_And_Identities_Array(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_Should_Return_Success_And_Identities_Array))]
        public async Task Theory()
        {
            // Arrange
            var client = await CreateAutheticatedClient();
            var poolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf");
            var identityId = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026");

            var expectedResult = new IdentityDto[]{
                new IdentityDto() {
                    Id = identityId,
                    Name = "Root Administrator",
                    Active = true,
                    CreatorId = identityId,
                    CreatedTime = new DateTimeOffset(2017, 7, 4, 1, 59, 59, 59, TimeSpan.FromHours(1)),
                }
            };

            // Act
            var response = await client.GetAsync($"/pools/{poolId}/identities");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var identities = DeserializeJson <IdentityDto[]>(content);

            identities.Should().NotBeNullOrEmpty();
            identities.Should().BeEquivalentTo(expectedResult);
        }
    }
}
