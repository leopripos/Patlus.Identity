using System.Collections.Generic;

namespace Patlus.IdentityManagement.Rest.Responses.Content
{
    public class ValidationErrorResultContent
    {
        public readonly string Message;
        public readonly IReadOnlyDictionary<string, List<string>> Details;

        public ValidationErrorResultContent(string message, IReadOnlyDictionary<string, List<string>> details)
        {
            this.Message = message;
            this.Details = details;
        }

        public ValidationErrorResultContent(string message) : this(message, new Dictionary<string, List<string>>())
        { }
    }
}
