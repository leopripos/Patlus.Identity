using System.Text.Json;

namespace Patlus.IdentityManagement.Rest.Filters.Actions
{
    public static class JsonNamingPolicies
    {
        public readonly static JsonNamingPolicy PascalCase = new PascalCaseNamingPolicy();
        public readonly static JsonNamingPolicy CamelCase = JsonNamingPolicy.CamelCase;
    }
}
