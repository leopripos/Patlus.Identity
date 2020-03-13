using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Me;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Me
{
    [Trait("IT-Feature", "/me")]
    [Trait("IT-Feature", "GET /me/profile")]
    public sealed class GET_PROFILE_Should_Return_Success_And_Profile_Dto : IntegrationTesting
    {
        public GET_PROFILE_Should_Return_Success_And_Profile_Dto(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(GET_PROFILE_Should_Return_Success_And_Profile_Dto))]
        public async Task Theory()
        {
            // Arrange
            var client = await CreateAutheticatedClient();
            var expectedResult = new ProfileDto
            {
                Id = new Guid("90fdc79d-b97a-4b62-9c04-5b2f94df2026"),
                Name = "Root Administrator",
                Active = true
            };

            // Act
            var response = await client.GetAsync("/me/profile");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var profile = DeserializeJson<ProfileDto>(content);

            profile.Should().BeEquivalentTo(expectedResult);


        }
    }
}
