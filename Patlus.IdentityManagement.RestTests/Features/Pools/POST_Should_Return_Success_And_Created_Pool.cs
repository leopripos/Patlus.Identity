using FluentAssertions;
using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Pools;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests.Features.Pools
{
    [Trait("IT-Feature", "/pools")]
    [Trait("IT-Feature", "POST /pools")]
    public sealed class POST_Should_Return_Success_And_Created_Pool : IntegrationTesting
    {
        public POST_Should_Return_Success_And_Created_Pool(TestWebApplicationFactory<Startup> factory)
            : base(factory)
        { }

        [Fact(DisplayName = nameof(POST_Should_Return_Success_And_Created_Pool))]
        public async Task Theory()
        {
            // Arrange
            var form = new CreateForm
            {
                Name = "New Name",
                Description = "New Pool Description",
                Active = true
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            // Act
            var response = await (await CreateAutheticatedClient()).PostAsync("/pools", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadAsStringAsync();
            var pool = DeserializeJson<PoolDto>(content);

            pool.Name.Should().Be(form.Name);
            pool.Description.Should().Be(form.Description);
            pool.Active.Should().Be(form.Active.Value);
        }

    }
}
