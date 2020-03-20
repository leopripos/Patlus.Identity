using System.Collections.Generic;

namespace Patlus.IdentityManagement.Rest.Responses.Content
{
    public class ValidationErrorDto
    {
        public string? Message { get; set; }
        public IDictionary<string, string[]>? Details { get; set; }

        public ValidationErrorDto() { }

        public ValidationErrorDto(string message, IDictionary<string, string[]> details)
        {
            this.Message = message;
            this.Details = details;
        }

        public ValidationErrorDto(Dictionary<string, string[]> details)
            : this("One or more validation errors occurred", details)
        { }

        public ValidationErrorDto(string message) : this(message, new Dictionary<string, string[]>())
        { }
    }
}
