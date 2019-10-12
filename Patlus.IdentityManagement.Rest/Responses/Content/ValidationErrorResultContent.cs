using System.Collections.Generic;

namespace Patlus.IdentityManagement.Rest.Responses.Content
{
    public class ValidationErrorResultContent
    {
        public string Message { get; set; }
        public IReadOnlyDictionary<string, string[]> Details { get; set; } = new Dictionary<string, string[]>();
    }
}
