using System.Text.Json;

namespace Patlus.Common.Presentation.Json
{
    public static class JsonNamingPolicies
    {
        public static readonly JsonNamingPolicy PascalCase = new PascalCaseNamingPolicy();
        public static readonly JsonNamingPolicy CamelCase = JsonNamingPolicy.CamelCase;
    }
}
