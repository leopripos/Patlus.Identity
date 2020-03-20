using Patlus.IdentityManagement.Rest;
using Patlus.IdentityManagement.Rest.Features.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Patlus.IdentityManagement.RestTests
{
    public class IntegrationTesting : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        protected readonly TestWebApplicationFactory<Startup> Factory;

        public IntegrationTesting(TestWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
        }

        protected HttpClient CreateClient()
        {
            var client = Factory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept-Case", "pascal");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        protected static string SerializeJson<TValue>(TValue model)
        {
            return JsonSerializer.Serialize(model);
        }

        [return: MaybeNull]
        protected static T DeserializeJson<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        protected async Task<HttpClient> CreateAutheticatedClient()
        {
            var form = new CreateForm
            {
                PoolId = new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"),
                Name = "root",
                Password = "root"
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                Encoding.UTF8,
                "application/json"
            );

            var response = await CreateClient().PostAsync("/tokens", httpContent);

            var tokenResult = DeserializeJson<TokenDto>(await response.Content.ReadAsStringAsync())!;

            return CreateAutheticatedClient(tokenResult.Scheme, tokenResult.Access);
        }

        protected HttpClient CreateAutheticatedClient(string scheme, string token)
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }

        protected Task<TokenDto?> CreateToken()
        {
            return CreateToken(new Guid("c73d72b1-326d-4213-ab11-ba47d83b9ccf"), "root", "root");
        }

        protected async Task<TokenDto?> CreateToken(Guid poolId, string name, string password)
        {
            var form = new CreateForm
            {
                PoolId = poolId,
                Name = name,
                Password = password
            };

            var httpContent = new StringContent(
                SerializeJson(form),
                UnicodeEncoding.UTF8,
                "application/json"
            );

            var response = await CreateClient().PostAsync("/tokens", httpContent);

            var content = await response.Content.ReadAsStringAsync();
            return DeserializeJson<TokenDto>(content);
        }
    }
}
